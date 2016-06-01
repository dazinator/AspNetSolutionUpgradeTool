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
            Type = PackageType.Default;
        }

        public List<string> OldNames { get; set; }

        public string Version { get; set; }

        public string Name { get; set; }

        public PackageType Type { get; set; }


    }

    public enum PackageType
    {
        Default,
        Build
    }
  
}