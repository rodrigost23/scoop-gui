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
                WorkingDirectory = System.IO.Directory.GetCurrentDirectory(),
                UseShellExecute = false
            };

            arguments ??= "";

            processStartInfo.Arguments = $" -noprofile -ex unrestricted \" & 'scoop.ps1' {arguments}; exit $lastexitcode\"";

            return processStartInfo;
        }

        public static async Task<string> RunAsync(string arguments)
        {
            try
            {
                using (Process p = new Process())
                {
                    p.StartInfo = Info(arguments);

                    var writer = new StreamWriter(stream);
                    await writer.WriteLineAsync("scoop {arguments}");

                    p.Start();
                    p.WaitForExit();

                    var result = p.StandardOutput.ReadToEnd();

                    await writer.WriteLineAsync(result);

                    return result;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static async Task<List<ScoopApp>> List()
        {
            var result = await RunAsync("list");

            var lines = result.Split("\n");

            var list = new List<ScoopApp>();

            foreach (string line in lines)
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
