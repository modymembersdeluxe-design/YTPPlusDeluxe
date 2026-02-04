# YTP+ Deluxe Edition (Windows Forms)

## Overview
YTP+ Deluxe Edition is a YouTube Poop (YTP) effects editor built with C# Windows Forms for Visual Studio 2022. It assembles clips, applies randomized audio/video effects, and renders results via external tools (FFmpeg/FFprobe/ImageMagick). This repository provides the project structure, core generator scaffolding, and UI wiring needed to evolve the editor into a full production build that supports YTP, YTP Tennis, Collab entries, and YTPMV workflows.

### Windows support targets
This scaffolding is intended to remain compatible with a broad range of Windows releases:
- Windows XP, Longhorn, Vista (beta/RTM)
- Windows 7 (beta/RTM), Windows 8 (beta/RTM), Windows 8.1
- Windows 10 (beta/RTM) and Windows 11

Actual compatibility depends on the .NET Framework runtime, FFmpeg build, and any third-party libraries you integrate.

### Deluxe GUI highlights
- Source browsers for local video/audio/images/GIFs/transition clips.
- URL list for online media inputs (download handled externally; URLs are registered in the project model).
- Preview button that tries `ffplay` first and falls back to `ffmpeg` if needed.
- Effect toggles (stutter, scramble, reverse, ear-rape, overlays, etc.) represented as pipeline flags in the generator model.
- Export via FFmpeg with multi-format output (`mp4`, `wmv`, `avi`, `mkv`).

### Features (implemented or scaffolded)
- Video/audio sources: `mp4`, `wmv`, `avi`, `mkv`, `mp3`, `wav`, `ogg` plus tracker best-effort (`xm`, `mod`, `it`).
- Image/GIF sources: `png`, `jpg`, `webp`, `gif`.
- Source Explorer (folder browse) and bulk add/tag workflow.
- Project types: Generic, YTP Tennis, Collab Entry, YTPMV.
- Generator: randomized automatic media “pooping” pipeline with concat/conversion steps.
- Output formats: `wmv`, `mp4`, `avi`, `mkv` (via FFmpeg).
- Mixer channels in the project model (Master, Aux1, Aux2, MIDI1-3).
- Min/max stream duration, clip count, and recall scaffolding.
- Asset folders: `images`, `memes`, `meme_sounds`, `sounds`, `overlay_videos`, `adverts`, `errors`.
- New effects that can consume asset folders (adverts, error overlays, meme overlays, etc.).
- Auto-keyframe metadata generation placeholder to aid Vegas editing.

### Audio effects (toggleable)
- Random sound, mute, speed up, slow down, reverse
- Chorus, vibrato, stutter, dance, squidward, sus, lagfun
- Low/high harmony, confusion, random chords, trailing reverses
- Low quality meme, audio crust, pitch-shifting loop, mashup mixing

### Video effects (toggleable)
- Invert, rainbow, mirror, mirror symmetry, screen clip
- Overlay images and sources, spadinner, sentence mixing
- Shuffle/loop frames, framerate reduction, random cuts
- Speed loop boost, scrambling/random chopping

### YTP style selection
- YTP 2007–2012 (classic)
- YTP 2013–2021 (modern)
- YTP Advance (experimental)

## Project Layout
- `YTPPlusDeluxe.csproj` — Windows Forms project file for VS2022.
- `Program.cs` — Application entry point.
- `ytpplus/Main.cs` + `ytpplus/Main.Designer.cs` — Main form UI and logic.
- `ytpplus/EffectsFactory.cs` — Effect catalog and FFmpeg filter builders.
- `ytpplus/ProjectModel.cs` — Project type, effect flags, asset folders, mixer channels.
- `ytpplus/Utilities.cs` — Helpers for file IO, randomization, and process execution.
- `ytpplus/YTPGenerator.cs` — Clip selection and render pipeline scaffolding.
- `App.config` — Application configuration.

## Notes
- This scaffolding assumes external tools are on PATH or configured in the UI: `ffmpeg.exe`, `ffprobe.exe`, and optionally `magick`.
- Effects are assembled as FFmpeg filter strings. Some effects are placeholders and should be replaced with production-ready algorithms.
