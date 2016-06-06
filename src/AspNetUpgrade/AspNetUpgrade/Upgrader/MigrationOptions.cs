namespace AspNetUpgrade.Upgrader
{
    public class MigrationOptions
    {
        public bool UpgradeProjectFilesToPreview1 { get; set; }

        public bool UpgradePackagesToRC2 { get; set; }

        public bool AddNetCoreTargetToApplications { get; set; }

        public bool AddNetStandardTargetToLibraries { get; set; }



    }
}