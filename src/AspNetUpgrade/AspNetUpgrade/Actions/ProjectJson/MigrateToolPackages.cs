using System.Collections.Generic;
using System.Linq;
using AspNetUpgrade.Model;
using AspNetUpgrade.Upgrader;
using Newtonsoft.Json.Linq;

namespace AspNetUpgrade.Actions.ProjectJson
{
    public class MigrateToolPackages : BaseMigrateSpecifiedPackages<ToolPackageMigrationInfo>
    {

        public MigrateToolPackages(List<ToolPackageMigrationInfo> targetPackages) : base(targetPackages)
        {
        }

        protected override JObject GetPackagesObject(IJsonProjectUpgradeContext fileUpgradeContext)
        {
            JObject projectJsonObject = fileUpgradeContext.JsonObject;
            JObject tools = (JObject)(projectJsonObject["tools"] ?? projectJsonObject["commands"]);
            return tools;
        }

        protected override void SetPackageProperty(JObject dependenciesObject, ToolPackageMigrationInfo targetPackage)
        {

           // var importsObj = new JObject(new JProperty("imports", importsArray));
            //importsObj.Add(importsArray);
          //  return new JProperty("netcoreapp1.0", importsObj);
            JObject depOpbject = new JObject();
            depOpbject.Add(new JProperty("version", targetPackage.Version));
            if (targetPackage.Imports.Any())
            {
                JArray importsArray = new JArray();
                foreach (var import in targetPackage.Imports)
                {
                    importsArray.Add(import);
                }
                //var importsString = string.Join("+", targetPackage.Imports);
                depOpbject.Add(new JProperty("imports", importsArray));
            }

            dependenciesObject[targetPackage.Name] = depOpbject;
        }
    }
}