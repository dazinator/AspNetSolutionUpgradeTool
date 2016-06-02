using System;

namespace AspNetUpgrade.Model
{
    public class DependencyPackageMigrationInfo : PackageMigrationInfo
    {

        public DependencyPackageMigrationInfo(string name, string version) : base(name, version)
        {
            Type = PackageType.Default;
        }

        public PackageType Type { get; set; }

    }
}