using System.IO;
using System.Linq;
using AspNetUpgrade.UpgradeContext;
using Microsoft.Build.Construction;

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
