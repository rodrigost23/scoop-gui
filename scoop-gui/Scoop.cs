using ScoopGui.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ScoopGui
{
    public static class Scoop
    {
        public static readonly Stream stream = new MemoryStream();

        private static ProcessStartInfo Info(string arguments = null)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = @"powershell.exe",
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                RedirectStandardOutput = true,
                WorkingDirectory = Directory.GetCurrentDirectory(),
                UseShellExecute = false
            };

            arguments ??= "";

            processStartInfo.Arguments = $" -noprofile -ex unrestricted \" & 'scoop.ps1' {arguments}; exit $lastexitcode\"";

            return processStartInfo;
        }

        public static async IAsyncEnumerable<string> RunAsync(string arguments)
        {
            using (Process p = new Process())
            {
                p.StartInfo = Info(arguments);

                var writer = new StreamWriter(stream);
                await writer.WriteLineAsync($"scoop {arguments}");

                p.Start();

                while (!p.StandardOutput.EndOfStream)
                {
                    var line = await p.StandardOutput.ReadLineAsync();
                    await writer.WriteLineAsync(line);

                    yield return line;
                }

                await p.WaitForExitAsync();
            }
        }

        public static async IAsyncEnumerable<ScoopApp> List()
        {
            string pattern = @"^(?<name>[\w\-]+)(?:\s+(?<version>[\w\.\-]+))?(?:\s+\[(?<bucket>.+)\])?(?:\s+(?<failed>\*failed\*))?$";
            await foreach (string line in RunAsync("list"))
            {
                var trimmed = line.Trim();
                if (trimmed == "Installed apps:" || trimmed == "")
                    continue;

                var groups = Regex.Match(trimmed, pattern).Groups;

                yield return new ScoopApp(name: groups["name"].Value)
                {
                    IsInstalled = true,
                    Version = groups["version"].Value,
                    Bucket = new ScoopBucket(groups["bucket"].Value),
                    IsFailed = groups["failed"].Value != ""
                };
            }
        }

        public static async IAsyncEnumerable<ScoopApp> Search(string query = null)
        {
            await foreach (string line in RunAsync("search" + (query ?? "")))
            {
                var trimmed = line.Trim();
                if (trimmed == "Installed apps:" || trimmed == "")
                    continue;

                yield return new ScoopApp(name: trimmed);
            }
        }
    }
}
