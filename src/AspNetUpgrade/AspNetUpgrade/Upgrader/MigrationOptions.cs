namespace AspNetUpgrade.Upgrader
{
    public class MigrationOptions
    {
        public MigrationOptions()
        {
            TargetFrameworkVersionForXprojFile = "v4.5";
        }

        public bool UpgradeProjectFilesToPreview1 { get; set; }

        public bool UpgradePackagesToRC2 { get; set; }

        public bool AddNetCoreTargetToApplications { get; set; }

        public bool AddNetStandardTargetToLibraries { get; set; }

        public string TargetFrameworkVersionForXprojFile { get; set; }



    }
}