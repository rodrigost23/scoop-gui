#nullable enable

namespace ScoopGui.Models
{
    public class ScoopBucket
    {
        public string Name { get; set; }

        public string? Repository { get; set; }

        public ScoopBucket(string name)
        {
            Name = name;
        }
        public ScoopBucket(string name, string repository)
        {
            Name = name;
            Repository = repository;
        }
        public override string ToString()
        {
            return Name;
        }
    }
}
