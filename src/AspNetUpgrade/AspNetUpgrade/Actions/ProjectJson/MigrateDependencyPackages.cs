using System.Collections.Generic;
using AspNetUpgrade.Model;
using AspNetUpgrade.Upgrader;
using Newtonsoft.Json.Linq;

namespace AspNetUpgrade.Actions.ProjectJson
{
    public class MigrateDependencyPackages : BaseMigrateSpecifiedPackages<DependencyPackageMigrationInfo>
    {

        public MigrateDependencyPackages(List<DependencyPackageMigrationInfo> targetPackages) : base(targetPackages)
        {
        }

        protected override JObject GetPackagesObject(IJsonProjectUpgradeContext fileUpgradeContext)
        {
            JObject projectJsonObject = fileUpgradeContext.JsonObject;
            JObject dependencies = (JObject)projectJsonObject["dependencies"];
            return dependencies;
        }

        protected override void SetPackageProperty(JObject dependenciesObject, DependencyPackageMigrationInfo targetPackage)
        {
            if (targetPackage.Type == PackageType.Build)
            {
                JObject depOpbject = new JObject();
                depOpbject.Add(new JProperty("version", targetPackage.Version));
                depOpbject.Add(new JProperty("type", targetPackage.Type.ToString().ToLowerInvariant()));
                dependenciesObject[targetPackage.Name] = depOpbject;
            }
            else
            {
                dependenciesObject[targetPackage.Name] = targetPackage.Version;
            }
        }
    }
}