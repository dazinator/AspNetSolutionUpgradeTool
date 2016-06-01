using System.Collections.Generic;

namespace AspNetUpgrade.Actions
{
    public class NuGetPackageInfo
    {

        public NuGetPackageInfo(string name, string version)
        {
            OldNames = new List<string>();
            Name = name;
            Version = version;
        }

        public List<string> OldNames { get; set; }

        public string Version { get; set; }

        public string Name { get; set; }


    }
}