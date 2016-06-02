using System.Collections.Generic;

namespace AspNetUpgrade.Model
{
    public class ToolPackageMigrationInfo : PackageMigrationInfo
    {

        public ToolPackageMigrationInfo(string name, string version) : base(name, version)
        {
            Imports = new List<string>();
        }

        public List<string> Imports { get; set; }

    }
}