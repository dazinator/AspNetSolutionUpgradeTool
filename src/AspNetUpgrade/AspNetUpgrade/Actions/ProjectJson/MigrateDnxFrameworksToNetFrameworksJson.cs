using System;
using System.Collections;
using System.ComponentModel;
using AspNetUpgrade.Model;
using AspNetUpgrade.UpgradeContext;
using Newtonsoft.Json.Linq;

namespace AspNetUpgrade.Actions.ProjectJson
{
    /// <summary>
    /// Renames dnx TFM's in project.json to be the .net TFM equivalent.
    /// </summary>
    public class MigrateDnxFrameworksToNetFramework452Json : IProjectUpgradeAction
    {

        public const string NetStandardLibraryVersion = "1.5.0-rc2-24027";
        public const string NetStandardTfm = "1.5.0-rc2-24027";
        public const string NetCoreAppTfmName = "netcoreapp1.0";
        public const string NetCoreAppVersion = "1.0.0-rc2-3002702";

        public void Apply(IProjectUpgradeContext fileUpgradeContext)
        {

            JObject projectJsonObject = fileUpgradeContext.ProjectJsonObject;
            JObject frameworks = (JObject)projectJsonObject["frameworks"];
            //_oldFrameworks = frameworks.DeepClone();

            // dnx451
            RenameFramework(frameworks, "dnx", "net452");
            RenameFramework(frameworks, "dnx45", "net452");
            RenameFramework(frameworks, "dnx451", "net452");
            RenameFramework(frameworks, "dnx452", "net452");

            // dnxCore
            var dnxCore50 = frameworks.Property("dnxcore50");
            if (dnxCore50 != null)
            {
                dnxCore50.Remove();

                var projWrapper = fileUpgradeContext.ToProjectJsonWrapper();
                var projType = projWrapper.GetProjectType();
                if (projType == ProjectType.Library)
                {
                    projWrapper.AddNetStandardFramework(NetStandardLibraryVersion, NetStandardTfm);
                }
                else if (projType == ProjectType.Application)
                {
                    projWrapper.AddNetCoreAppFramework(NetCoreAppVersion, NetCoreAppTfmName);
                }
              
            }

        }

        private static void RenameFramework(JObject frameworks, string oldName, string newName)
        {
            var dnx = frameworks.Property(oldName);
            if (dnx != null)
            {
                // consider auto migrating dnx451 to net452 - See comments on https://github.com/aspnet/Home/issues/1381
                var renamed = dnx.Rename(name => name == oldName ? newName : name);
                dnx.Replace(renamed);
            }
        }
    }
}
