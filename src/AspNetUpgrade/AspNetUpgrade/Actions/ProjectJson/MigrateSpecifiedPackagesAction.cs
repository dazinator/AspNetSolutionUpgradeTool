using System.Collections.Generic;
using AspNetUpgrade.Model;
using AspNetUpgrade.Upgrader;
using Newtonsoft.Json.Linq;

namespace AspNetUpgrade.Actions.ProjectJson
{
    public class MigrateSpecifiedPackages : IProjectJsonUpgradeAction
    {

        private JToken _backup;

        private List<NuGetPackageMigrationInfo> _targetPackages;

        public MigrateSpecifiedPackages(List<NuGetPackageMigrationInfo> targetPackages)
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

                switch (targetPackage.MigrationAction)
                {
                    case PackageMigrationAction.Remove:
                        RemovePackage(targetPackage, dependencies);
                        continue;

                    case PackageMigrationAction.Update:
                        UpdatePackage(targetPackage, dependencies);
                        continue;

                    case PackageMigrationAction.AddOrUpdate:
                        AddOrUpdatePackage(targetPackage, dependencies);
                        continue;
                }

            }
        }

        private void AddOrUpdatePackage(NuGetPackageMigrationInfo targetPackage, JObject dependenciesObject)
        {
            // Update package if exists.
            UpdatePackage(targetPackage, dependenciesObject);

            // if not present, then add it.
            var updatedPackage = dependenciesObject[targetPackage.Name];
            if (updatedPackage == null)
            {
                // add it.
                SetDependency(dependenciesObject, targetPackage);
            }
        }

        private void SetDependency(JObject dependenciesObject, NuGetPackageMigrationInfo targetPackage)
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

        private void UpdatePackage(NuGetPackageMigrationInfo targetPackage, JObject dependenciesObject)
        {
            // if package is present, then update it.
            var dependency = dependenciesObject[targetPackage.Name];
            if (dependency != null)
            {
                SetDependency(dependenciesObject, targetPackage);
                return;
            }

            // pacakge not found under current name, check for old package names and update if found.
            foreach (var oldPackageName in targetPackage.OldNames)
            {
                dependency = dependenciesObject[oldPackageName];
                if (dependency != null)
                {
                    // rename to current name.
                    dependency.Rename(targetPackage.Name);
                    //todo: if the type is build, need to expand the property value to be an object with version property and type property
                    SetDependency(dependenciesObject, targetPackage);
                    break;
                }
            }
        }

        private void RemovePackage(NuGetPackageMigrationInfo targetPackage, JObject dependenciesObject)
        {
            // remove this package if it is present (by its latest name or any old names).
            var dependency = dependenciesObject[targetPackage.Name];
            if (dependency != null)
            {
                dependenciesObject.Remove(targetPackage.Name);
                return;
            }

            foreach (var oldPackageName in targetPackage.OldNames)
            {
                dependency = dependenciesObject[oldPackageName];
                if (dependency != null)
                {
                    dependenciesObject.Remove(targetPackage.Name);
                    return;
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