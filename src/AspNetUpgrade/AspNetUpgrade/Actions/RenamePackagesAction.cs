using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace AspNetUpgrade.Actions
{
    public class MigrateSpecifiedPackagesAction : IAction
    {

        private JToken _backup;

        private List<NuGetPackageMigrationInfo> _targetPackages;

        public MigrateSpecifiedPackagesAction(List<NuGetPackageMigrationInfo> targetPackages)
        {
            _targetPackages = targetPackages;
        }

        public void Apply(IJsonFileUpgradeContext fileUpgradeContext)
        {
            JObject projectJsonObject = fileUpgradeContext.ProjectJsonObject;
            JObject dependencies = (JObject)projectJsonObject["dependencies"];
            _backup = dependencies.DeepClone();

            foreach (var targetPackage in _targetPackages)
            {
                foreach (var oldPackageName in targetPackage.OldNames)
                {
                    var dependency = dependencies[oldPackageName];
                    if (dependency != null)
                    {
                        dependency.Rename(targetPackage.Name);
                        //todo: if the type is build, need to expand the property value to be an object with version property and type property
                        if (targetPackage.Type == PackageType.Build)
                        {
                            JObject depOpbject = new JObject();
                            depOpbject.Add(new JProperty("version", targetPackage.Version));
                            depOpbject.Add(new JProperty("type", targetPackage.Type.ToString().ToLowerInvariant()));
                            dependencies[targetPackage.Name] = depOpbject;
                        }
                        else
                        {
                            dependencies[targetPackage.Name] = targetPackage.Version;
                        }
                        
                        break;
                    }
                }

            }
        }

        public void Undo(IJsonFileUpgradeContext fileUpgradeContext)
        {
            // restore frameworks section
            JObject projectJsonObject = fileUpgradeContext.ProjectJsonObject;
            projectJsonObject["dependencies"].Replace(_backup);

        }
    }
}