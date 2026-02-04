using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace YTPPlusDeluxe
{
    public partial class YTPPlusDeluxe : Form
    {
        private bool renderComplete = true;
        private readonly ProjectModel projectModel = new ProjectModel();

        // Tool variables
        private string ffmpeg = "ffmpeg.exe";
        private string ffprobe = "ffprobe.exe";
        private string magick = "magick";
        private string temp = "temp\\";
        private string sounds = "sounds\\";
        private string music = "music\\";
        private string resources = "resources\\";
        private string[] sources = Array.Empty<string>();

        // Default variables
        private bool transitionsDef = true;
        private bool introBoolDef;
        private bool outroBoolDef;
        private bool pluginTestDef;
        private int clipCountDef = 20;
        private int widthDef = 640;
        private int heightDef = 480;
        private decimal minStreamDef = 0.2M;
        private decimal maxStreamDef = 0.4M;
        private string introDef = "resources\\intro.mp4";
        private string outroDef = "resources\\outro.mp4";
        private string ffmpegDef = "ffmpeg.exe";
        private string ffprobeDef = "ffprobe.exe";
        private string magickDef = "magick";
        private string sourcesDef = "sources\\";
        private string tempDef = "temp\\";
        private string soundsDef = "sounds\\";
        private string musicDef = "music\\";
        private string resourcesDef = "resources\\";

        private YTPGenerator? globalGen;
        private int pluginCount;
        private readonly List<string> enabledPlugins = new List<string>();
        private readonly PresenceClient client = new PresenceClient();
        private readonly Timestamps timestamps = new Timestamps();
        private readonly string[] titles =
        {
            "Yo",
            "Mmmmm!",
            "I'm the invisible man...",
            "Luigi, look!",
            "You want it?",
            "WTF Booooooooooom"
        };

        // Change the following field declarations to nullable types to resolve CS8618
        private ComboBox? projectTypeCombo;

        // Update the property to handle the nullable field
        public ComboBox? ProjectTypeCombo => projectTypeCombo;

        public YTPPlusDeluxe()
        {
            InitializeComponent();
            SetVars();
            ProjectTypeCombo.SelectedIndex = 0;
            OutputFormat.SelectedIndex = 0;
        }

        public void ResetVars()
        {
            InsertTransitions.Checked = transitionsDef;
            pluginTest.Checked = pluginTestDef;
            InsertIntro.Checked = introBoolDef;
            InsertOutro.Checked = outroBoolDef;
            Clips.Value = clipCountDef;
            WidthSet.Value = widthDef;
            HeightSet.Value = heightDef;
            MinStreamDur.Value = minStreamDef;
            MaxStreamDur.Value = maxStreamDef;
            Intro.Text = introDef;
            Outro.Text = outroDef;
            ffmpeg = ffmpegDef;
            ffprobe = ffprobeDef;
            magick = magickDef;
            TransitionDir.Text = sourcesDef;
            temp = tempDef;
            sounds = soundsDef;
            music = musicDef;
            resources = resourcesDef;

            pluginCount = 0;
            enabledPlugins.Clear();

            projectModel.VideoSources.Clear();
            projectModel.AudioSources.Clear();
            projectModel.ImageSources.Clear();
            projectModel.GifSources.Clear();
            projectModel.TransitionSources.Clear();
            projectModel.UrlSources.Clear();
            VideoSources.Items.Clear();
            AudioSources.Items.Clear();
            ImageSources.Items.Clear();
            GifSources.Items.Clear();
            TransitionSources.Items.Clear();
            UrlSources.Items.Clear();
        }

        public void SetVars()
        {
            ResetVars();
        }

        public void TestMagick()
        {
            if (!Utilities.TryRunProcess(magick, "-version", out _))
            {
                alert("ImageMagick is not installed. The Squidward effect has been disabled.\n" +
                      "Please install ImageMagick and add it to your system PATH, or select \"Set magick.exe\" in the Tools menu.");
                effect_Squidward.Enabled = false;
                effect_Squidward.Checked = false;
            }
            else
            {
                effect_Squidward.Enabled = true;
                effect_Squidward.Checked = true;
            }
        }

        public void TestFFMPEG()
        {
            if (!Utilities.TryRunProcess(ffmpeg, "-version", out _))
            {
                alert("FFMPEG is not installed. It may have been misplaced; make sure it is in the same directory as YTP++!");
                Environment.Exit(1);
            }
        }

        public void TestFFPROBE()
        {
            if (!Utilities.TryRunProcess(ffprobe, "-version", out _))
            {
                alert("FFPROBE is not installed. It may have been misplaced; make sure it is in the same directory as YTP++!");
                Environment.Exit(1);
            }
        }

        public void alert(string alertText)
        {
            MessageBox.Show(this, alertText, titles[new Random().Next(0, titles.Length)]);
        }

        public void addSource()
        {
            using (var dialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "Video Files (*.mp4)|*.mp4"
            })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    foreach (var file in dialog.FileNames)
                    {
                        Array.Resize(ref sources, sources.Length + 1);
                        sources[sources.Length - 1] = file;
                        Material.Text += file + Environment.NewLine;
                    }
                }
            }
        }

        public void clearSources()
        {
            sources = Array.Empty<string>();
            Material.Text = string.Empty;
        }

        private static List<string> GetItems(ListBox listBox)
        {
            var items = new List<string>();
            foreach (var item in listBox.Items)
            {
                if (item is string value)
                {
                    items.Add(value);
                }
            }

            return items;
        }

        private EffectFlags CollectEffectFlags()
        {
            var flags = EffectFlags.None;
            if (effect_Reverse.Checked) flags |= EffectFlags.Reverse;
            if (effect_Stutter.Checked) flags |= EffectFlags.Stutter;
            if (effect_Earrape.Checked) flags |= EffectFlags.LowQuality;
            if (effect_Rainbow.Checked) flags |= EffectFlags.Rainbow;
            if (effect_Mirror.Checked) flags |= EffectFlags.Mirror;
            if (effect_Sus.Checked) flags |= EffectFlags.Sus;
            return flags;
        }

        private void AddSourceToList(OpenFileDialog dialog, ListBox listBox, List<string> storage)
        {
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            foreach (var file in dialog.FileNames)
            {
                storage.Add(file);
                listBox.Items.Add(file);
            }
        }

        private void AddSourceFromFolder(string description, string[] extensions, ListBox listBox, List<string> storage)
        {
            using (var dialog = new FolderBrowserDialog
            {
                Description = description
            })
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                foreach (var file in Directory.GetFiles(dialog.SelectedPath))
                {
                    if (Array.Exists(extensions, ext => file.EndsWith(ext, StringComparison.OrdinalIgnoreCase)))
                    {
                        storage.Add(file);
                        listBox.Items.Add(file);
                    }
                }
            }
        }

        private static long nanoTime()
        {
            long nano = 10000L * Stopwatch.GetTimestamp();
            nano /= TimeSpan.TicksPerMillisecond;
            nano *= 100L;
            return nano;
        }

        public void progress(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            client.SetPresence(new RichPresence
            {
                Details = titles[new Random().Next(0, titles.Length)],
                Assets = new Assets
                {
                    LargeImageKey = "icon",
                    LargeImageText = "YTP+, made with ❤ in C#."
                },
                State = "Rendering... (" + e.ProgressPercentage + "%)",
                Timestamps = timestamps
            });
        }

        public void complete(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBar1.Value = 100;
            renderComplete = true;
            SaveAs.Enabled = true;
            Render.Enabled = true;

            if (globalGen != null && globalGen.failed)
            {
                client.SetPresence(new RichPresence
                {
                    Details = titles[new Random().Next(0, titles.Length)],
                    Assets = new Assets
                    {
                        LargeImageKey = "icon",
                        LargeImageText = "YTP+, made with ❤ in C#."
                    },
                    State = "Render failed",
                    Timestamps = timestamps
                });
                alert("An exception has occured during rendering. Rendering may have not produced a result.\n\n" +
                      "The last exception to occur was:\n" + globalGen.exc.Message);
            }
            else
            {
                client.SetPresence(new RichPresence
                {
                    Details = titles[new Random().Next(0, titles.Length)],
                    Assets = new Assets
                    {
                        LargeImageKey = "icon",
                        LargeImageText = "YTP+, made with ❤ in C#."
                    },
                    State = "Render complete",
                    Timestamps = timestamps
                });
            }
        }

        public void RenderVideo()
        {
            if (sources.Length == 0 && !InsertTransitions.Checked)
            {
                alert("You need some sources...");
                return;
            }

            try
            {
                client.SetPresence(new RichPresence
                {
                    Details = titles[new Random().Next(0, titles.Length)],
                    Assets = new Assets
                    {
                        LargeImageKey = "icon",
                        LargeImageText = "YTP+, made with ❤ in C#."
                    },
                    State = "Rendering...",
                    Timestamps = timestamps
                });

                renderComplete = false;
                SaveAs.Enabled = false;
                Render.Enabled = false;

                projectModel.Type = (ProjectType)ProjectTypeCombo.SelectedIndex;
                projectModel.OutputFormat = OutputFormat.SelectedItem?.ToString() ?? "mp4";
                projectModel.EnabledEffects = CollectEffectFlags();
                projectModel.VideoSources.Clear();
                projectModel.VideoSources.AddRange(sources);
                projectModel.VideoSources.AddRange(GetItems(VideoSources));
                projectModel.AudioSources.Clear();
                projectModel.AudioSources.AddRange(GetItems(AudioSources));
                projectModel.ImageSources.Clear();
                projectModel.ImageSources.AddRange(GetItems(ImageSources));
                projectModel.GifSources.Clear();
                projectModel.GifSources.AddRange(GetItems(GifSources));
                projectModel.TransitionSources.Clear();
                projectModel.TransitionSources.AddRange(GetItems(TransitionSources));
                projectModel.UrlSources.Clear();
                projectModel.UrlSources.AddRange(GetItems(UrlSources));

                var outputPath = Path.Combine(temp, $"tempoutput.{projectModel.OutputFormat}");
                var generator = new YTPGenerator(outputPath)
                {
                    insertTransitionClips = InsertTransitions.Checked,
                    width = Convert.ToInt32(WidthSet.Value, CultureInfo.InvariantCulture),
                    height = Convert.ToInt32(HeightSet.Value, CultureInfo.InvariantCulture),
                    intro = InsertIntro.Checked,
                    outro = InsertOutro.Checked,
                    pluginTest = pluginTest.Checked,
                    pluginCount = pluginCount,
                    plugins = enabledPlugins
                };

                generator.ApplyProjectModel(projectModel);
                generator.toolBox.FFMPEG = Utilities.Quote(ffmpeg);
                generator.toolBox.FFPROBE = Utilities.Quote(ffprobe);
                generator.toolBox.MAGICK = Utilities.Quote(magick);
                generator.toolBox.TEMP = temp;
                generator.toolBox.SOUNDS = sounds;
                generator.toolBox.MUSIC = music;
                generator.toolBox.RESOURCES = resources;
                generator.toolBox.SOURCES = TransitionDir.Text;
                generator.toolBox.intro = Intro.Text;
                generator.toolBox.outro = Outro.Text;

                foreach (var sourcem in sources)
                {
                    generator.addSource(Utilities.Quote(sourcem));
                }

                generator.setMaxClips(Convert.ToInt32(Clips.Value, CultureInfo.InvariantCulture));
                generator.setMaxDuration(Convert.ToDouble(MaxStreamDur.Value, CultureInfo.InvariantCulture));
                generator.setMinDuration(Convert.ToDouble(MinStreamDur.Value, CultureInfo.InvariantCulture));

                _ = nanoTime();
                globalGen = generator.go(progress, complete);
            }
            catch (Exception ex)
            {
                Render.Enabled = true;
                Console.WriteLine(ex.StackTrace);
            }
        }

        private void Render_Click(object sender, EventArgs e)
        {
            RenderVideo();
        }

        private void AddMaterial_Click(object sender, EventArgs e)
        {
            addSource();
        }

        private void ClearMaterial_Click(object sender, EventArgs e)
        {
            clearSources();
        }

        private void PausePlay_Click(object sender, EventArgs e)
        {
            PausePlay.Text = PausePlay.Text == "▶️" ? "| |" : "▶️";
        }

        private void Start_Click(object sender, EventArgs e)
        {
            PausePlay.Text = "| |";
        }

        private void End_Click(object sender, EventArgs e)
        {
            PausePlay.Text = "▶️";
        }

        private void AddVideo_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "Video Files (*.mp4;*.wmv;*.avi;*.mkv)|*.mp4;*.wmv;*.avi;*.mkv"
            })
            {
                AddSourceToList(dialog, VideoSources, projectModel.VideoSources);
            }
        }

        private void AddAudio_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "Audio Files (*.mp3;*.wav;*.ogg;*.xm;*.mod;*.it)|*.mp3;*.wav;*.ogg;*.xm;*.mod;*.it"
            })
            {
                AddSourceToList(dialog, AudioSources, projectModel.AudioSources);
            }
        }

        private void AddImage_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "Image Files (*.png;*.jpg;*.webp)|*.png;*.jpg;*.webp"
            })
            {
                AddSourceToList(dialog, ImageSources, projectModel.ImageSources);
            }
        }

        private void AddGif_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "GIF Files (*.gif)|*.gif"
            })
            {
                AddSourceToList(dialog, GifSources, projectModel.GifSources);
            }
        }

        private void AddTransition_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "Transition Videos (*.mp4;*.wmv;*.avi;*.mkv)|*.mp4;*.wmv;*.avi;*.mkv"
            })
            {
                AddSourceToList(dialog, TransitionSources, projectModel.TransitionSources);
            }
        }

        private void AddUrl_Click(object sender, EventArgs e)
        {
            var url = InputBox("Enter a media URL (YouTube, Facebook, etc.):", "Add URL");
            if (!string.IsNullOrWhiteSpace(url))
            {
                projectModel.UrlSources.Add(url);
                UrlSources.Items.Add(url);
            }
        }

        private void Preview_Click(object sender, EventArgs e)
        {
            var selected = VideoSources.SelectedItem?.ToString()
                           ?? AudioSources.SelectedItem?.ToString()
                           ?? ImageSources.SelectedItem?.ToString()
                           ?? GifSources.SelectedItem?.ToString()
                           ?? TransitionSources.SelectedItem?.ToString();

            if (string.IsNullOrWhiteSpace(selected))
            {
                alert("Select a source to preview.");
                return;
            }

            if (!Utilities.TryStartPreview(selected, "ffplay", ffmpeg, out var error))
            {
                alert("Preview failed to start: " + error);
            }
        }

        private static string InputBox(string prompt, string title)
        {
            string result = string.Empty;
            using (var form = new Form())
            {
                form.Text = title;
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.StartPosition = FormStartPosition.CenterParent;
                form.MinimizeBox = false;
                form.MaximizeBox = false;
                form.ShowInTaskbar = false;
                form.ClientSize = new System.Drawing.Size(420, 120);

                var label = new Label
                {
                    Left = 10,
                    Top = 10,
                    Text = prompt,
                    AutoSize = true
                };
                var textBox = new TextBox
                {
                    Left = 10,
                    Top = 30,
                    Width = 400
                };
                var ok = new Button
                {
                    Text = "OK",
                    Left = 240,
                    Width = 80,
                    Top = 60,
                    DialogResult = DialogResult.OK
                };
                var cancel = new Button
                {
                    Text = "Cancel",
                    Left = 330,
                    Width = 80,
                    Top = 60,
                    DialogResult = DialogResult.Cancel
                };

                form.Controls.Add(label);
                form.Controls.Add(textBox);
                form.Controls.Add(ok);
                form.Controls.Add(cancel);
                form.AcceptButton = ok;
                form.CancelButton = cancel;

                if (form.ShowDialog() == DialogResult.OK)
                {
                    result = textBox.Text;
                }
            }

            return result;
        }
    }
}
