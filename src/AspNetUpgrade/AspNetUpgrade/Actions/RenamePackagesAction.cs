using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace AspNetUpgrade.Actions
{
    public class RenamePackagesAction : IAction
    {

        private JToken _backup;

        private List<NuGetPackageInfo> _targetPackages;

        public RenamePackagesAction(List<NuGetPackageInfo> targetPackages)
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
                        dependencies[targetPackage.Name] = targetPackage.Version;
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