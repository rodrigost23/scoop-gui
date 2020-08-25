using ScoopGui.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

        public static async Task<List<ScoopApp>> List()
        {
            var list = new List<ScoopApp>();

            await foreach (string line in RunAsync("list"))
            {
                var trimmed = line.Trim();
                if (trimmed == "Installed apps:" || trimmed == "")
                    continue;

                list.Add(new ScoopApp { name = trimmed });
            }

            return list;
        }

        public static async Task<List<ScoopApp>> Search(string query = null)
        {
            var list = new List<ScoopApp>();

            await foreach (string line in RunAsync("search" + (query ?? "")))
            {
                var trimmed = line.Trim();
                if (trimmed == "Installed apps:" || trimmed == "")
                    continue;

                list.Add(new ScoopApp { name = trimmed });
            }

            return list;
        }
    }
}
