using System;
using System.Collections;
using System.ComponentModel;
using AspNetUpgrade.UpgradeContext;
using Newtonsoft.Json.Linq;

namespace AspNetUpgrade.Actions.ProjectJson
{
    /// <summary>
    /// Renames dnx TFM's in project.json to be the .net TFM equivalent.
    /// </summary>
    public class MigrateDnxFrameworksToNetFrameworksJson : IProjectUpgradeAction
    {
        //private JToken _oldFrameworks;



        public void Apply(IProjectUpgradeContext fileUpgradeContext)
        {

            JObject projectJsonObject = fileUpgradeContext.JsonObject;
            JObject frameworks = (JObject)projectJsonObject["frameworks"];
            //_oldFrameworks = frameworks.DeepClone();

            // dnx451
            RenameFramework(frameworks, "dnx", "net45");
            RenameFramework(frameworks, "dnx45", "net45");
            RenameFramework(frameworks, "dnx451", "net451");
            RenameFramework(frameworks, "dnx452", "net452");

            // dnxCore
            //var dnxCore50 = frameworks.Property("dnxcore50");
            //if (dnxCore50 != null)
            //{
            //    dnxCore50.Remove();
            //    var netCoreObject = BuildNetCoreFrameworkDependencyJson(fileUpgradeContext);
            //    frameworks.Add(netCoreObject);
            //}

        }

        private static void RenameFramework(JObject frameworks, string oldName, string newName)
        {
            var dnx = frameworks.Property(oldName);
            if (dnx != null)
            {
                //  dnx451.Remove();
                // consider auto migrating dnx451 to net452 - See comments on https://github.com/aspnet/Home/issues/1381
                var renamed = dnx.Rename(name => name == oldName ? newName : name);
                dnx.Replace(renamed);
            }
        }

        //public void Undo(IJsonProjectUpgradeContext fileUpgradeContext)
        //{
        //    // restore frameworks section
        //    JObject projectJsonObject = fileUpgradeContext.JsonObject;
        //    projectJsonObject["frameworks"].Replace(_oldFrameworks);

        //}
    }
}
