using System.IO;
using System.Linq;
using AspNetUpgrade.UpgradeContext;
using Microsoft.Build.Construction;

namespace AspNetUpgrade.Actions.Xproj
{
    public class SetTargetFrameworkVersion : IProjectUpgradeAction
    {
        private string _targetFramework;

        public SetTargetFrameworkVersion(string targetFramework)
        {
            _targetFramework = targetFramework;
        }

        public void Apply(IProjectUpgradeContext fileUpgradeContext)
        {
            if (fileUpgradeContext.VsProjectFile != null)
            {
                fileUpgradeContext.VsProjectFile.AddPropertyValue("Globals", "TargetFrameworkVersion", _targetFramework);
            }
        }
    }
}
