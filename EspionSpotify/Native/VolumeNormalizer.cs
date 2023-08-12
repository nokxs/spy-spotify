using EspionSpotify.Properties;
using System;
using System.Diagnostics;
using System.Text;

namespace EspionSpotify.Native
{
    internal class VolumeNormalizer
    {
        private const int DEFAULT_DB = 89;

        public void Normalize(string file)
        {
            var targetDb = Settings.Default.advanced_normalize_db_target;
            var dbOffset = targetDb - DEFAULT_DB;

            //Process.Start("mp3gain.exe", $"/c /g {dbOffset}").WaitForExit();

            // Create a new process start info
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "mp3gain.exe",
                Arguments = $"/c /g {dbOffset} \"{file}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            // Create the process and start it
            using (Process process = new Process())
            {
                process.StartInfo = startInfo;

                StringBuilder standardOutput = new StringBuilder();
                StringBuilder standardError = new StringBuilder();

                // Define event handlers to capture output and error data
                process.OutputDataReceived += (sender, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                    {
                        standardOutput.AppendLine(e.Data);
                    }
                };

                process.ErrorDataReceived += (sender, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                    {
                        standardError.AppendLine(e.Data);
                    }
                };

                process.Start();

                // Begin asynchronous reading of output and error streams
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                process.WaitForExit(); // Wait for the process to complete

                // Retrieve the captured output and error
                string capturedOutput = standardOutput.ToString();
                string capturedError = standardError.ToString();

                Console.WriteLine("Captured Output:");
                Console.WriteLine(capturedOutput);

                Console.WriteLine("Captured Error:");
                Console.WriteLine(capturedError);
            }
        }
    }
}
