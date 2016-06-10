using System.Collections.Generic;
using System.Diagnostics;
using AspNetUpgrade.Model;
using AspNetUpgrade.UpgradeContext;
using Newtonsoft.Json.Linq;

namespace AspNetUpgrade.Actions.ProjectJson
{
    public class MigrateDependencyPackages : BaseMigrateSpecifiedPackages<DependencyPackageMigrationInfo>
    {

        public MigrateDependencyPackages(List<DependencyPackageMigrationInfo> targetPackages) : base(targetPackages)
        {
        }

        protected override JObject GetPackagesObject(IProjectUpgradeContext fileUpgradeContext)
        {
            JObject projectJsonObject = fileUpgradeContext.ProjectJsonObject;
            var dependencies = projectJsonObject.GetOrAddProperty("dependencies", null);
            if (dependencies == null)
            {
                Debugger.Break();
            }
           // JObject dependencies = (JObject)projectJsonObject["dependencies"];
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