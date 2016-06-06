using System.IO;
using System.Linq;
using Microsoft.Build.Construction;

namespace AspNetUpgrade.Actions.Xproj
{
    public static class InstallTargetsHelper
    {
        

        public static void UpgradeDnxProjectToDotNet(Microsoft.Build.Evaluation.Project project, bool isWeb)
        {
            // _Logger.LogInfo(string.Format("Project Dir is: {0}", projectDir));
            ReplaceProjectImport(project, "Microsoft.DNX.Props", "$(VSToolsPath)\\DotNet\\Microsoft.DotNet.Props");

            if (isWeb)
            {
                ReplaceProjectImport(project, "Microsoft.DNX.targets", "$(VSToolsPath)\\DotNet.Web\\Microsoft.DotNet.Web.targets");
            }
            else
            {
                ReplaceProjectImport(project, "Microsoft.DNX.targets", "$(VSToolsPath)\\DotNet\\Microsoft.DotNet.targets");
            }
        }


        private static void ReplaceProjectImport(Microsoft.Build.Evaluation.Project project, string oldImportProjectName, string newImportProjectName)
        {
            // Ensure import is present, replace existing if found.
            var fileName = Path.GetFileName(oldImportProjectName);
            var existingImport = project.Xml.Imports.Cast<ProjectImportElement>().FirstOrDefault(i => i.Project.EndsWith(fileName));
            if (existingImport != null)
            {
                existingImport.Project = newImportProjectName;
                //  _Logger.LogInfo(string.Format("The existing import will be upgraded for: {0}", fileName));
                // project.Xml.RemoveChild(existingImport);
            }

        }

    }
}