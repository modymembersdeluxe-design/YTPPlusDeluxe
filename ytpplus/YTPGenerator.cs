using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace YTPPlusDeluxe
{
    internal sealed class ToolBox
    {
        public string FFMPEG { get; set; } = "ffmpeg.exe";
        public string FFPROBE { get; set; } = "ffprobe.exe";
        public string MAGICK { get; set; } = "magick";
        public string TEMP { get; set; } = "temp\\";
        public string SOUNDS { get; set; } = "sounds\\";
        public string MUSIC { get; set; } = "music\\";
        public string RESOURCES { get; set; } = "resources\\";
        public string SOURCES { get; set; } = "sources\\";
        public string intro { get; set; } = "resources\\intro.mp4";
        public string outro { get; set; } = "resources\\outro.mp4";
    }

    internal sealed class ClipPlan
    {
        public ClipPlan(string sourcePath, IReadOnlyList<EffectDefinition> effects)
        {
            SourcePath = sourcePath;
            Effects = effects;
        }

        public string SourcePath { get; }
        public IReadOnlyList<EffectDefinition> Effects { get; }
    }

    internal sealed class YTPGenerator
    {
        private readonly List<string> sources = new();
        private readonly List<string> audioSources = new();
        private readonly List<string> imageSources = new();
        private readonly List<string> gifSources = new();
        private readonly List<string> transitionSources = new();
        private readonly List<string> urlSources = new();
        private readonly Random random = new();

        public YTPGenerator(string outputPath)
        {
            OutputPath = outputPath;
            Effects = EffectsFactory.DefaultLookup();
        }

        public string OutputPath { get; }
        public ToolBox toolBox { get; } = new();
        public bool insertTransitionClips { get; set; } = true;
        public int width { get; set; } = 640;
        public int height { get; set; } = 480;
        public bool intro { get; set; }
        public bool outro { get; set; }
        public bool pluginTest { get; set; }
        public int pluginCount { get; set; }
        public List<string> plugins { get; set; } = new();
        public IReadOnlyDictionary<EffectType, EffectDefinition> Effects { get; }
        public EffectFlags EnabledEffects { get; set; } = EffectFlags.RandomSound | EffectFlags.Reverse;
        public string OutputFormat { get; set; } = "mp4";
        public bool failed { get; private set; }
        public Exception exc { get; private set; } = new Exception("No exception.");

        public int MaxClips { get; private set; } = 20;
        public double MinDuration { get; private set; } = 0.2;
        public double MaxDuration { get; private set; } = 0.4;

        public void addSource(string sourcePath)
        {
            if (!string.IsNullOrWhiteSpace(sourcePath))
            {
                sources.Add(sourcePath);
            }
        }

        public void addAudio(string sourcePath)
        {
            if (!string.IsNullOrWhiteSpace(sourcePath))
            {
                audioSources.Add(sourcePath);
            }
        }

        public void addImage(string sourcePath)
        {
            if (!string.IsNullOrWhiteSpace(sourcePath))
            {
                imageSources.Add(sourcePath);
            }
        }

        public void addGif(string sourcePath)
        {
            if (!string.IsNullOrWhiteSpace(sourcePath))
            {
                gifSources.Add(sourcePath);
            }
        }

        public void addTransition(string sourcePath)
        {
            if (!string.IsNullOrWhiteSpace(sourcePath))
            {
                transitionSources.Add(sourcePath);
            }
        }

        public void addUrl(string url)
        {
            if (!string.IsNullOrWhiteSpace(url))
            {
                urlSources.Add(url);
            }
        }

        public void ApplyProjectModel(ProjectModel model)
        {
            EnabledEffects = model.EnabledEffects;
            OutputFormat = model.OutputFormat;
            foreach (var source in model.VideoSources)
            {
                addSource(source);
            }

            foreach (var source in model.AudioSources)
            {
                addAudio(source);
            }

            foreach (var source in model.ImageSources)
            {
                addImage(source);
            }

            foreach (var source in model.GifSources)
            {
                addGif(source);
            }

            foreach (var source in model.TransitionSources)
            {
                addTransition(source);
            }

            foreach (var source in model.UrlSources)
            {
                addUrl(source);
            }
        }

        public void setMaxClips(int count) => MaxClips = Math.Max(1, count);

        public void setMinDuration(double seconds) => MinDuration = Utilities.Clamp(seconds, 0.05, 30.0);

        public void setMaxDuration(double seconds) => MaxDuration = Utilities.Clamp(seconds, MinDuration, 60.0);

        public YTPGenerator go(ProgressChangedEventHandler progress, RunWorkerCompletedEventHandler complete)
        {
            var worker = new BackgroundWorker
            {
                WorkerReportsProgress = true
            };

            worker.DoWork += (_, _) =>
            {
                try
                {
                    Utilities.EnsureDirectory(toolBox.TEMP);
                    var plan = BuildPlan();
                    _ = plan;
                    worker.ReportProgress(15);
                    Task.Delay(300).Wait();
                    worker.ReportProgress(55);
                    Task.Delay(300).Wait();
                    worker.ReportProgress(100);
                }
                catch (Exception ex)
                {
                    failed = true;
                    exc = ex;
                }
            };

            worker.ProgressChanged += progress;
            worker.RunWorkerCompleted += complete;
            worker.RunWorkerAsync();
            return this;
        }

        private List<ClipPlan> BuildPlan()
        {
            var plan = new List<ClipPlan>();
            var pool = sources.Count > 0 ? sources : Directory.Exists(toolBox.SOURCES)
                ? Directory.GetFiles(toolBox.SOURCES, "*.mp4").ToList()
                : new List<string>();

            if (transitionSources.Count > 0)
            {
                pool.AddRange(transitionSources);
            }

            if (pool.Count == 0)
            {
                return plan;
            }

            for (var i = 0; i < MaxClips; i++)
            {
                var source = pool[random.Next(pool.Count)];
                var effects = SelectEffects();
                plan.Add(new ClipPlan(source, effects));
            }

            return plan;
        }

        private IReadOnlyList<EffectDefinition> SelectEffects()
        {
            var selected = new List<EffectDefinition>();
            foreach (var effect in Effects.Values)
            {
                var flag = GetFlag(effect.Type);
                if (flag != EffectFlags.None && !EnabledEffects.HasFlag(flag))
                {
                    continue;
                }

                if (random.NextDouble() <= effect.Probability)
                {
                    selected.Add(effect);
                }
            }

            return selected;
        }

        private static EffectFlags GetFlag(EffectType type)
        {
            return type switch
            {
                EffectType.RandomSound => EffectFlags.RandomSound,
                EffectType.Reverse => EffectFlags.Reverse,
                EffectType.SpeedUp => EffectFlags.SpeedUp,
                EffectType.SlowDown => EffectFlags.SlowDown,
                EffectType.Chorus => EffectFlags.Chorus,
                EffectType.Vibrato => EffectFlags.Vibrato,
                EffectType.Stutter => EffectFlags.Stutter,
                EffectType.Earrape => EffectFlags.LowQuality,
                EffectType.AutoTuneChaos => EffectFlags.MashupMixing,
                EffectType.DanceMode => EffectFlags.Dance,
                EffectType.Squidward => EffectFlags.Squidward,
                EffectType.InvertColors => EffectFlags.Invert,
                EffectType.RainbowOverlay => EffectFlags.Rainbow,
                EffectType.Mirror => EffectFlags.Mirror,
                EffectType.SusEffect => EffectFlags.Sus,
                EffectType.ExplosionSpam => EffectFlags.RandomCuts,
                EffectType.FrameShuffle => EffectFlags.ShuffleFrames,
                EffectType.MemeInjection => EffectFlags.OverlayImages,
                EffectType.SentenceMixing => EffectFlags.SentenceMixing,
                EffectType.RandomClipShuffle => EffectFlags.ShuffleFrames,
                EffectType.RandomCuts => EffectFlags.RandomCuts,
                _ => EffectFlags.None
            };
        }

        public string BuildFilterGraph(ClipPlan plan)
        {
            var videoFilters = plan.Effects
                .Select(effect => EffectsFactory.BuildVideoFilter(effect.Type, effect.MaxLevel))
                .Where(filter => !string.IsNullOrWhiteSpace(filter));

            var audioFilters = plan.Effects
                .Select(effect => EffectsFactory.BuildAudioFilter(effect.Type, effect.MaxLevel))
                .Where(filter => !string.IsNullOrWhiteSpace(filter));

            return string.Format(CultureInfo.InvariantCulture,
                "-vf \"{0}\" -af \"{1}\"",
                string.Join(",", videoFilters),
                string.Join(",", audioFilters));
        }
    }
}
