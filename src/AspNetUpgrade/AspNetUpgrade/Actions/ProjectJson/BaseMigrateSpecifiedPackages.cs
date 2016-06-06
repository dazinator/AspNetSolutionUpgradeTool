using System.Collections.Generic;
using AspNetUpgrade.Model;
using AspNetUpgrade.UpgradeContext;
using Newtonsoft.Json.Linq;

namespace AspNetUpgrade.Actions.ProjectJson
{
    public abstract class BaseMigrateSpecifiedPackages<T> : IProjectUpgradeAction
        where T: PackageMigrationInfo
    {
       
        private List<T> _targetPackages;

        public BaseMigrateSpecifiedPackages(List<T> targetPackages)
        {
            _targetPackages = targetPackages;
        }

        protected abstract JObject GetPackagesObject(IProjectUpgradeContext fileUpgradeContext);
        

        public void Apply(IProjectUpgradeContext fileUpgradeContext)
        {
            JObject packagesObject = GetPackagesObject(fileUpgradeContext);

            foreach (var targetPackage in _targetPackages)
            {

                switch (targetPackage.MigrationAction)
                {
                    case PackageMigrationAction.Remove:
                        RemovePackage(targetPackage, packagesObject);
                        continue;

                    case PackageMigrationAction.Update:
                        UpdatePackage(targetPackage, packagesObject);
                        continue;

                    case PackageMigrationAction.AddOrUpdate:
                        AddOrUpdatePackage(targetPackage, packagesObject);
                        continue;
                }

            }
        }

        protected virtual void AddOrUpdatePackage(T targetPackage, JObject dependenciesObject)
        {
            // Update package if exists.
            UpdatePackage(targetPackage, dependenciesObject);

            // if not present, then add it.
            var updatedPackage = dependenciesObject[targetPackage.Name];
            if (updatedPackage == null)
            {
                // add it.
                SetPackageProperty(dependenciesObject, targetPackage);
            }
        }

        protected abstract void SetPackageProperty(JObject dependenciesObject, T targetPackage);
       

        private void UpdatePackage(T targetPackage, JObject dependenciesObject)
        {
            // if package is present, then update it.
            var dependency = dependenciesObject[targetPackage.Name];
            if (dependency != null)
            {
                SetPackageProperty(dependenciesObject, targetPackage);
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
                    SetPackageProperty(dependenciesObject, targetPackage);
                    break;
                }
            }
        }

        private void RemovePackage(T targetPackage, JObject dependenciesObject)
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
    }
}