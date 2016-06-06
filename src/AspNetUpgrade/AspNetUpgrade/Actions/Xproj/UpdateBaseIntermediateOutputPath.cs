using AspNetUpgrade.UpgradeContext;

namespace AspNetUpgrade.Actions.Xproj
{
    public class UpdateBaseIntermediateOutputPath : IProjectUpgradeAction
    {
        public void Apply(IProjectUpgradeContext fileUpgradeContext)
        {
            if (fileUpgradeContext.VsProjectFile != null)
            {
                fileUpgradeContext.VsProjectFile.UpdatePropertyValue("BaseIntermediateOutputPath", ".\\obj");
            }
        }

       
    }
}
