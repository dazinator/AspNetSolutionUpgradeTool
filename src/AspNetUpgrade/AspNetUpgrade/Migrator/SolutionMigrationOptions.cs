namespace AspNetUpgrade.Migrator
{
    public class SolutionMigrationOptions
    {

        public SolutionMigrationOptions()
        {
            UpgradeToPreview1 = true;
            SdkVersionNumber = "1.0.0-preview1-002702";
        }

        public bool UpgradeToPreview1 { get; set; }
        
        public string SdkVersionNumber { get; set; }


    }
}