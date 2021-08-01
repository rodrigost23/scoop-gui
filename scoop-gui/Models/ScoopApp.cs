#nullable enable

namespace ScoopGui.Models
{
    public class ScoopApp
    {
        public string Name { get; set; }

        public string? Version { get; set; }

        public ScoopBucket? Bucket { get; set; }

        public bool? IsInstalled { get; set; }

        public bool IsFailed { get; set; }
        public bool IsHold { get; set; }

        public ScoopApp(string name)
        {
            Name = name;
        }
    }
}
