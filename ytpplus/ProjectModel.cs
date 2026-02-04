using System;
using System.Collections.Generic;

namespace YTPPlusDeluxe
{
    internal enum ProjectType
    {
        Generic,
        YtpTennis,
        CollabEntry,
        YtpMv
    }

    [Flags]
    internal enum EffectFlags
    {
        None = 0,
        RandomSound = 1 << 0,
        Mute = 1 << 1,
        SpeedUp = 1 << 2,
        SlowDown = 1 << 3,
        Reverse = 1 << 4,
        Chorus = 1 << 5,
        Vibrato = 1 << 6,
        Stutter = 1 << 7,
        Dance = 1 << 8,
        Squidward = 1 << 9,
        Sus = 1 << 10,
        LagFun = 1 << 11,
        LowHarmony = 1 << 12,
        HighHarmony = 1 << 13,
        Confusion = 1 << 14,
        RandomChords = 1 << 15,
        TrailingReverses = 1 << 16,
        LowQuality = 1 << 17,
        AudioCrust = 1 << 18,
        PitchLoop = 1 << 19,
        MashupMixing = 1 << 20,
        Invert = 1 << 21,
        Rainbow = 1 << 22,
        Mirror = 1 << 23,
        MirrorSymmetry = 1 << 24,
        ScreenClip = 1 << 25,
        OverlayImages = 1 << 26,
        OverlaySources = 1 << 27,
        Spadinner = 1 << 28,
        SentenceMixing = 1 << 29,
        ShuffleFrames = 1 << 30,
        RandomCuts = 1 << 31
    }

    internal sealed class MixerChannel
    {
        public MixerChannel(string name)
        {
            Name = name;
        }

        public string Name { get; }
        public double Volume { get; set; } = 1.0;
        public bool Muted { get; set; }
    }

    internal sealed class ProjectModel
    {
        public ProjectType Type { get; set; } = ProjectType.Generic;
        public EffectFlags EnabledEffects { get; set; } = EffectFlags.RandomSound | EffectFlags.Reverse;
        public string OutputFormat { get; set; } = "mp4";

        public List<string> VideoSources { get; } = new List<string>();
        public List<string> AudioSources { get; } = new List<string>();
        public List<string> ImageSources { get; } = new List<string>();
        public List<string> GifSources { get; } = new List<string>();
        public List<string> TransitionSources { get; } = new List<string>();
        public List<string> UrlSources { get; } = new List<string>();

        public Dictionary<string, string> AssetFolders { get; } = new Dictionary<string, string>
        {
            ["images"] = "images",
            ["memes"] = "memes",
            ["meme_sounds"] = "meme_sounds",
            ["sounds"] = "sounds",
            ["overlay_videos"] = "overlay_videos",
            ["adverts"] = "adverts",
            ["errors"] = "errors"
        };

        public List<MixerChannel> MixerChannels { get; } = new List<MixerChannel>
        {
            new MixerChannel("Master"),
            new MixerChannel("Aux1"),
            new MixerChannel("Aux2"),
            new MixerChannel("MIDI1"),
            new MixerChannel("MIDI2"),
            new MixerChannel("MIDI3")
        };
    }
}
