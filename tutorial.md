# YTP+ Deluxe Edition Tutorial

## Getting Started (Modern Windows)
1. Install .NET Framework 4.8 (or the highest supported runtime for your OS).
2. Install FFmpeg and FFprobe, and ensure they are available on your PATH.
3. (Optional) Install ImageMagick for effects that depend on `magick`.
4. Open `YTP+ Deluxe.sln` in Visual Studio 2022.
5. Build the solution and launch.

## Basic Workflow
- Use the Source browsers to add video/audio/image/GIF/transition clips.
- Add URLs for online sources (download externally, then import the files).
- Choose a Project Type and Output Format.
- Toggle effects as desired and click **Preview** to test clips.
- Click **Render** to generate output, then **Save As** to export.

## Tips
- Keep effects minimal for faster iterations.
- Use smaller clip counts when testing render settings.
- Verify FFmpeg is configured in the Tools menu if preview/export fails.
