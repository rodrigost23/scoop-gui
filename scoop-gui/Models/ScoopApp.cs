#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoopGui.Models
{
    public class ScoopApp
    {
        public string Name { get; set; }

        public string? Version { get; set; }

        public ScoopBucket? Bucket { get; set; }

        public bool? IsInstalled { get; set; }

        public bool IsFailed { get; set; } = false;

        public ScoopApp(string name)
        {
            Name = name;
        }
    }
}
