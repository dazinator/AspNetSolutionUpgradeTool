using AspNetUpgrade.Model;

namespace AspNetUpgrade.Migrator
{
    public class SolutionMigrationOptions
    {

        public SolutionMigrationOptions(ToolingVersion toolingVersion)
        {
           ToolingVersion = toolingVersion;
            if (ToolingVersion == ToolingVersion.Preview1)
            {
                SdkVersionNumber = "1.0.0-preview1-002702";
            }
            else if (ToolingVersion == ToolingVersion.Preview2)
            {
                SdkVersionNumber = "1.0.0-preview2-003121";
            }
        }

        public ToolingVersion ToolingVersion { get; set; }

        public string SdkVersionNumber { get; set; }


    }
}