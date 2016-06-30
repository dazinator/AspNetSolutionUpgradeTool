using AspNetUpgrade.Model;

namespace AspNetUpgrade.Migrator
{
    public class ProjectMigrationOptions
    {
        public ProjectMigrationOptions(ReleaseVersion version)
        {
            TargetFrameworkVersionForXprojFile = "v4.5";
            ToolingVersion = ToolingVersion;
            PackagesVersion = version;

            if (version == ReleaseVersion.RC2)
            {
                ToolingVersion = ToolingVersion.Preview1;
            }
            else if (version == ReleaseVersion.RTM)
            {
                ToolingVersion = ToolingVersion.Preview2;
            }
            // AddNetCoreTargetForApplications = true;
            // AddNetStandardTargetForLibraries = true;
        }

        public ReleaseVersion PackagesVersion { get; set; }
        public ToolingVersion ToolingVersion { get; set; }

        public bool AddNetCoreTargetForApplications { get; set; }

        public bool AddNetStandardTargetForLibraries { get; set; }

        /// <summary>
        /// Only relevant if upgrade to preview 1 enabled.
        /// </summary>
        public string TargetFrameworkVersionForXprojFile { get; set; }

    }
}