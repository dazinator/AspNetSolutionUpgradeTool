namespace AspNetUpgrade.Migrator
{
    public class ProjectMigrationOptions
    {
        public ProjectMigrationOptions()
        {
            TargetFrameworkVersionForXprojFile = "v4.5";
            UpgradeToPreview1 = true;
            UpgradePackagesToRc2 = true;
           // AddNetCoreTargetForApplications = true;
           // AddNetStandardTargetForLibraries = true;
        }

        public bool UpgradeToPreview1 { get; set; }

        public bool UpgradePackagesToRc2 { get; set; }

        public bool AddNetCoreTargetForApplications { get; set; }

        public bool AddNetStandardTargetForLibraries { get; set; }

        /// <summary>
        /// Only relevant if upgrade to preview 1 enabled.
        /// </summary>
        public string TargetFrameworkVersionForXprojFile { get; set; }

    }
}