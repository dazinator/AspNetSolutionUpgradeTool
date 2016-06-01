using System.Collections.Generic;

namespace AspNetUpgrade.Model
{
    public class NuGetPackageMigrationInfo
    {

        public NuGetPackageMigrationInfo(string name, string version)
        {
            OldNames = new List<string>();
            Name = name;
            Version = version;
            Type = PackageType.Default;
        }

        public List<string> OldNames { get; set; }

        public string Version { get; set; }

        public string Name { get; set; }

        public PackageType Type { get; set; }


    }
}