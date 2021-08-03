#nullable enable

using PropertyChanged.SourceGenerator;

namespace ScoopGui.Models
{
    public partial class ScoopApp
    {
        public string Name { get; set; }
        public ScoopBucket? Bucket { get; set; }
        [Notify] private string? _version;
        [Notify] private string? _versionUpstream;
        [Notify] private bool? _isInstalled;
        [Notify] private bool? _isFailed;
        [Notify] private bool? _isHold;

        public bool? IsUpdatable => IsInstalled == null || Version == null || VersionUpstream == null ? null
                                    : (Version != VersionUpstream);

        public ScoopApp(string name)
        {
            Name = name;
        }
    }
}
