using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace YTPPlusDeluxe
{
    internal static class Utilities
    {
        public static string Quote(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return "\"\"";
            }

            return value.IndexOf(" ", StringComparison.Ordinal) >= 0
                ? "\"" + value + "\""
                : value;
        }

        public static void EnsureDirectory(string path)
        {
            if (!string.IsNullOrWhiteSpace(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public static bool TryRunProcess(string fileName, string arguments, out string output)
        {
            output = string.Empty;
            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = fileName,
                    Arguments = arguments,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };

                var process = Process.Start(startInfo);
                if (process == null)
                {
                    output = "Process could not be started.";
                    return false;
                }

                using (process)
                {
                    output = process.StandardOutput.ReadToEnd();
                    output += process.StandardError.ReadToEnd();
                    process.WaitForExit();
                    return process.ExitCode == 0;
                }
            }
            catch (Exception ex)
            {
                output = ex.Message;
                return false;
            }
        }

        public static bool TryStartProcess(string fileName, string arguments, out string error)
        {
            error = string.Empty;
            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = fileName,
                    Arguments = arguments,
                    CreateNoWindow = true,
                    UseShellExecute = false
                };

                var process = Process.Start(startInfo);
                if (process == null)
                {
                    error = "Process could not be started.";
                    return false;
                }

                // Dispose the Process instance since caller only needs it started.
                process.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        public static bool TryStartPreview(string filePath, string ffplayPath, string ffmpegPath, out string error)
        {
            var quotedFile = Quote(filePath);
            if (TryStartProcess(ffplayPath, $"-autoexit {quotedFile}", out error))
            {
                return true;
            }

            return TryStartProcess(ffmpegPath, $"-i {quotedFile} -f sdl \"{filePath}\"", out error);
        }

        public static int Clamp(int value, int min, int max) => Math.Max(min, Math.Min(max, value));

        public static double Clamp(double value, double min, double max) => Math.Max(min, Math.Min(max, value));

        public static string FormatTime(double seconds) => seconds.ToString("0.00", CultureInfo.InvariantCulture);
    }

    internal sealed class PresenceClient
    {
        public void SetPresence(RichPresence presence)
        {
            _ = presence;
        }
    }

    internal sealed class RichPresence
    {
        public string Details { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public Assets Assets { get; set; } = new Assets();
        public Timestamps Timestamps { get; set; } = new Timestamps();
    }

    internal sealed class Assets
    {
        public string LargeImageKey { get; set; } = string.Empty;
        public string LargeImageText { get; set; } = string.Empty;
    }

    internal sealed class Timestamps
    {
        public DateTime Start { get; set; } = DateTime.UtcNow;
    }
}
