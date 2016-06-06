using System.IO;
using System.Linq;
using AspNetUpgrade.Upgrader;
using Microsoft.Build.Construction;

namespace AspNetUpgrade.Actions.Xproj
{
    public class MigrateProjectImportsFromDnxToDotNet : IProjectUpgradeAction
    {
        public void Apply(IJsonProjectUpgradeContext fileUpgradeContext)
        {
            if (fileUpgradeContext.VsProjectFile != null)
            {
                bool isWebProject = fileUpgradeContext.ToProjectJsonWrapper().IsMvcProject();
                UpgradeDnxProjectToDotNet(fileUpgradeContext.VsProjectFile, isWebProject);
            }
        }

        public static void UpgradeDnxProjectToDotNet(Microsoft.Build.Evaluation.Project project, bool isWeb)
        {
            // _Logger.LogInfo(string.Format("Project Dir is: {0}", projectDir));
            project.UpdateImport("Microsoft.DNX.Props", "$(VSToolsPath)\\DotNet\\Microsoft.DotNet.Props");

            if (isWeb)
            {
                project.UpdateImport("Microsoft.DNX.targets", "$(VSToolsPath)\\DotNet.Web\\Microsoft.DotNet.Web.targets");
            }
            else
            {
                project.UpdateImport("Microsoft.DNX.targets", "$(VSToolsPath)\\DotNet\\Microsoft.DotNet.targets");
            }
        }



    }
}
