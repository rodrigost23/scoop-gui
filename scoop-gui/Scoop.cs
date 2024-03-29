﻿using ScoopGui.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ScoopGui
{
    public static class Scoop
    {
        public static readonly Stream stream = new MemoryStream();

        private static ProcessStartInfo Info(string arguments = null)
        {
            ProcessStartInfo processStartInfo = new()
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
            using Process p = new();

            p.StartInfo = Info(arguments);

            StreamWriter writer = new(stream);
            await writer.WriteLineAsync($"scoop {arguments}");

            _ = p.Start();

            while (!p.StandardOutput.EndOfStream)
            {
                string line = await p.StandardOutput.ReadLineAsync();
                await writer.WriteLineAsync(line);

                yield return line;
            }

            await p.WaitForExitAsync();
        }

        public static async IAsyncEnumerable<ScoopApp> List()
        {
            string pattern = @"^(?<name>[\w\-]+)(?:\s+(?<version>[\w\.\-]+))?(?:\s+\*(?<flag>.+)\*)?(?:\s+\[(?<bucket>.+)\])?$";
            await foreach (string line in RunAsync("list"))
            {
                string trimmed = line.Trim();

                GroupCollection groups = Regex.Match(trimmed, pattern).Groups;

                if (string.IsNullOrEmpty(groups["name"].Value))
                {
                    continue;
                }

                yield return new ScoopApp(name: groups["name"].Value)
                {
                    IsInstalled = true,
                    Version = groups["version"].Value,
                    Bucket = new ScoopBucket(groups["bucket"].Value),
                    IsFailed = groups["flag"].Value == "failed",
                    IsHold = groups["flag"].Value == "hold"
                };
            }
        }
        public static async IAsyncEnumerable<ScoopApp> Status()
        {
            string pattern = @"^\s*(?<name>[\w\-]+)\:\s*(?<version>[\w\.\-]+)\s*\->\s*(?<versionUpstream>[\w\.\-]+)$";
            await foreach (string line in RunAsync("status"))
            {
                string trimmed = line.Trim();

                GroupCollection groups = Regex.Match(trimmed, pattern).Groups;

                if (string.IsNullOrEmpty(groups["name"].Value))
                {
                    continue;
                }

                yield return new ScoopApp(name: groups["name"].Value)
                {
                    IsInstalled = true,
                    Version = groups["version"].Value,
                    VersionUpstream = groups["versionUpstream"].Value,
                    Bucket = new ScoopBucket(groups["bucket"].Value),
                    IsFailed = groups["flag"].Value == "failed",
                    IsHold = groups["flag"].Value == "hold"
                };
            }
        }

        public static async IAsyncEnumerable<ScoopApp> Search(string query = null)
        {
            string pattern = @"(?<bucket>.+?)\/(?<name>.+)\n *Version: (?<version>.+)(?:\n *Description: (?<description>.*))?";

            string result = string.Join("\n", await RunAsync("search " + (query ?? "")).ToListAsync());

            MatchCollection matches = Regex.Matches(result, pattern);

            foreach (Match match in matches)
            {
                GroupCollection groups = match.Groups;

                yield return new ScoopApp(name: groups["name"].Value)
                {
                    VersionUpstream = groups["version"].Value,
                    Bucket = new ScoopBucket(groups["bucket"].Value)
                };
            }
        }

        public static async IAsyncEnumerable<ScoopBucket> BucketList()
        {
            await foreach (string line in RunAsync("bucket list"))
            {
                string trimmed = line.Trim();
                yield return new ScoopBucket(name: trimmed);
            }
        }

        public static async IAsyncEnumerable<ScoopBucket> BucketKnown()
        {
            await foreach (string line in RunAsync("bucket known"))
            {
                string trimmed = line.Trim();
                yield return new ScoopBucket(name: trimmed);
            }
        }
    }
}
