using System;
using System.Collections.Generic;
using System.Linq;

namespace YTPPlusDeluxe
{
    internal enum EffectType
    {
        RandomSound,
        Reverse,
        SpeedUp,
        SlowDown,
        Chorus,
        Vibrato,
        Stutter,
        Earrape,
        AutoTuneChaos,
        DanceMode,
        Squidward,
        InvertColors,
        RainbowOverlay,
        Mirror,
        SusEffect,
        ExplosionSpam,
        FrameShuffle,
        MemeInjection,
        SentenceMixing,
        RandomClipShuffle,
        RandomCuts
    }

    internal sealed class EffectDefinition
    {
        public EffectDefinition(EffectType type, string displayName, double probability, int maxLevel)
        {
            Type = type;
            DisplayName = displayName;
            Probability = probability;
            MaxLevel = maxLevel;
        }

        public EffectType Type { get; }
        public string DisplayName { get; }
        public double Probability { get; set; }
        public int MaxLevel { get; set; }
    }

    internal static class EffectsFactory
    {
        public static IReadOnlyList<EffectDefinition> DefaultEffects() => new List<EffectDefinition>
        {
            new EffectDefinition(EffectType.RandomSound, "Random Sound", 0.35, 2),
            new EffectDefinition(EffectType.Reverse, "Reverse Clip", 0.2, 1),
            new EffectDefinition(EffectType.SpeedUp, "Speed Up", 0.25, 3),
            new EffectDefinition(EffectType.SlowDown, "Slow Down", 0.25, 3),
            new EffectDefinition(EffectType.Chorus, "Chorus", 0.2, 2),
            new EffectDefinition(EffectType.Vibrato, "Vibrato/Pitch Bend", 0.2, 2),
            new EffectDefinition(EffectType.Stutter, "Stutter Loop", 0.25, 3),
            new EffectDefinition(EffectType.Earrape, "Earrape Mode", 0.1, 2),
            new EffectDefinition(EffectType.AutoTuneChaos, "Auto-Tune Chaos", 0.05, 1),
            new EffectDefinition(EffectType.DanceMode, "Dance & Squidward", 0.2, 2),
            new EffectDefinition(EffectType.Squidward, "Squidward", 0.15, 2),
            new EffectDefinition(EffectType.InvertColors, "Invert Colors", 0.2, 1),
            new EffectDefinition(EffectType.RainbowOverlay, "Rainbow Overlay", 0.2, 2),
            new EffectDefinition(EffectType.Mirror, "Mirror Mode", 0.2, 1),
            new EffectDefinition(EffectType.SusEffect, "Sus Effect", 0.15, 2),
            new EffectDefinition(EffectType.ExplosionSpam, "Explosion Spam", 0.1, 2),
            new EffectDefinition(EffectType.FrameShuffle, "Frame Shuffle", 0.1, 1),
            new EffectDefinition(EffectType.MemeInjection, "Meme Injection", 0.2, 2),
            new EffectDefinition(EffectType.SentenceMixing, "Sentence Mixing", 0.25, 2),
            new EffectDefinition(EffectType.RandomClipShuffle, "Random Clip Shuffle", 0.25, 2),
            new EffectDefinition(EffectType.RandomCuts, "Random Cuts", 0.25, 2)
        };

        public static IReadOnlyDictionary<EffectType, EffectDefinition> DefaultLookup() =>
            DefaultEffects().ToDictionary(effect => effect.Type);

        public static string BuildAudioFilter(EffectType type, int level)
        {
            level = Utilities.Clamp(level, 1, 5);

            return type switch
            {
                EffectType.RandomSound => "adelay=0|0",
                EffectType.Reverse => "areverse",
                EffectType.SpeedUp => $"atempo={1.1 + (0.15 * level):0.00}",
                EffectType.SlowDown => $"atempo={1.0 - (0.12 * level):0.00}",
                EffectType.Chorus => "aecho=0.8:0.88:60:0.4",
                EffectType.Vibrato => "asetrate=48000*1.1,atempo=0.909",
                EffectType.Stutter => "aselect='not(between(t,0,0.08))',asetpts=N/SR/TB",
                EffectType.Earrape => $"volume={4 + level * 2}",
                EffectType.AutoTuneChaos => "afftdn,compand",
                EffectType.SusEffect => "atempo=0.95,asetrate=44100*1.03",
                EffectType.SentenceMixing => "asegment,aselect='gt(random(0),0.5)'",
                EffectType.RandomClipShuffle => "aselect='gt(random(1),0.4)',asetpts=N/SR/TB",
                EffectType.RandomCuts => "atrim=0:1.0,asetpts=N/SR/TB",
                _ => string.Empty
            };
        }

        public static string BuildVideoFilter(EffectType type, int level)
        {
            level = Utilities.Clamp(level, 1, 5);

            return type switch
            {
                EffectType.Reverse => "reverse",
                EffectType.SpeedUp => $"setpts={1.0 / (1.1 + 0.15 * level):0.00}*PTS",
                EffectType.SlowDown => $"setpts={(1.1 + 0.15 * level):0.00}*PTS",
                EffectType.DanceMode => "hue=s=2,eq=contrast=1.3:saturation=1.6",
                EffectType.Squidward => "gblur=sigma=6",
                EffectType.InvertColors => "negate",
                EffectType.RainbowOverlay => "format=rgba,colorchannelmixer=rr=1:gg=1:bb=1",
                EffectType.Mirror => "hflip",
                EffectType.ExplosionSpam => "boxblur=2:2",
                EffectType.FrameShuffle => "shuffleframes",
                EffectType.MemeInjection => "overlay=10:10",
                EffectType.SentenceMixing => "select='gt(random(0),0.5)',setpts=N/FRAME_RATE/TB",
                EffectType.RandomClipShuffle => "select='gt(random(1),0.4)',setpts=N/FRAME_RATE/TB",
                EffectType.RandomCuts => "trim=0:1.0,setpts=PTS-STARTPTS",
                _ => string.Empty
            };
        }
    }
}
