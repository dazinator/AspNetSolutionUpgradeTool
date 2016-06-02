using System.Collections.Generic;

namespace AspNetUpgrade.Model
{
    public abstract class PackageMigrationInfo
    {
        public PackageMigrationInfo(string name, string version)
        {
            OldNames = new List<string>();
            Name = name;
            Version = version;
            MigrationAction = PackageMigrationAction.Update;
        }

        /// <summary>
        /// If packages are found with these old names, then they will be updated to be the current name and version.
        /// </summary>
        public List<string> OldNames { get; set; }

        public string Version { get; set; }

        public string Name { get; set; }

        public PackageMigrationAction MigrationAction { get; set; }
    }
}