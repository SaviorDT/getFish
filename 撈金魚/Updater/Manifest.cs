using System.Collections.Generic;

namespace 撈金魚.Updater
{
    internal class ManifestEntry
    {
        public string Version { get; set; }
        public string Url { get; set; }
    }

    internal class Manifest
    {
        public List<ManifestEntry> Versions { get; set; }
    }
}
