using System.IO;
using System.Linq;
using AspNetUpgrade.Upgrader;
using Microsoft.Build.Construction;

namespace AspNetUpgrade.Actions.Xproj
{
    public class UpdateBaseIntermediateOutputPath : IJsonUpgradeAction
    {
        public void Apply(IJsonProjectUpgradeContext fileUpgradeContext)
        {
            if (fileUpgradeContext.VsProjectFile != null)
            {
                fileUpgradeContext.VsProjectFile.UpdatePropertyValue("BaseIntermediateOutputPath", ".\\obj");
            }
        }

       
    }
}
