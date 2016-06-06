using System.Collections.Generic;
using System.Linq;
using AspNetUpgrade.Model;
using AspNetUpgrade.UpgradeContext;
using Newtonsoft.Json.Linq;

namespace AspNetUpgrade.Actions.ProjectJson
{
    public class MigrateToolPackages : BaseMigrateSpecifiedPackages<ToolPackageMigrationInfo>
    {

        public MigrateToolPackages(List<ToolPackageMigrationInfo> targetPackages) : base(targetPackages)
        {
        }

        protected override JObject GetPackagesObject(IProjectUpgradeContext fileUpgradeContext)
        {
            JObject projectJsonObject = fileUpgradeContext.JsonObject;
            JObject tools = (JObject)(projectJsonObject["tools"] ?? projectJsonObject["commands"] ?? projectJsonObject.GetOrAddProperty("tools", null));
            return tools;
        }

        protected override void SetPackageProperty(JObject dependenciesObject, ToolPackageMigrationInfo targetPackage)
        {
            JObject depOpbject = new JObject();
            depOpbject.Add(new JProperty("version", targetPackage.Version));
            if (targetPackage.Imports.Any())
            {
                JArray importsArray = new JArray();
                foreach (var import in targetPackage.Imports)
                {
                    importsArray.Add(import);
                }
                depOpbject.Add(new JProperty("imports", importsArray));
            }

            dependenciesObject[targetPackage.Name] = depOpbject;
        }
    }
}