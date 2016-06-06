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
                UpdatePropertyValue(fileUpgradeContext.VsProjectFile, "BaseIntermediateOutputPath", ".\\obj");
            }
        }

        public static void UpdatePropertyValue(Microsoft.Build.Evaluation.Project project, string propertyName, string value)
        {
            // _Logger.LogInfo(string.Format("Project Dir is: {0}", projectDir));
            var existingProp = project.Xml.Properties.Cast<ProjectPropertyElement>().FirstOrDefault(i => i.Name.ToLowerInvariant() == propertyName.ToLowerInvariant());
            if (existingProp != null)
            {
                existingProp.Value = value;
            }
        }
    }
}
