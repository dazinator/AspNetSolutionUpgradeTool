using System;
using System.Collections.Generic;
using AspNetUpgrade.Migrator;
using AspNetUpgrade.Model;
using AspNetUpgrade.UpgradeContext;
using Newtonsoft.Json.Linq;

namespace AspNetUpgrade.Actions.ProjectJson
{
    public abstract class BaseDependenciesUpdate : IProjectUpgradeAction
    {
        
        private string[] _excludePackageNames;
        private string _packagePrefixToUpgrade;

        private string _oldVersionLabelContainsThis;
        private string _newVersionNumber;

        private Func<ReleaseVersion, JProperty, bool> _dependencyFilter;
        private Action<ReleaseVersion,JObject, JProperty> _updateDependencyCallback;

      //  private ReleaseVersion _version;
        
        protected BaseDependenciesUpdate(ReleaseVersion releaseVersion, Func<ReleaseVersion,JProperty, bool> dependencyPredicate, Action<ReleaseVersion,JObject, JProperty> updateDependencyCallback)
        {
            ReleaseVersion = releaseVersion;
            _dependencyFilter = dependencyPredicate;
            _updateDependencyCallback = updateDependencyCallback;
        }

        public void Apply(IProjectUpgradeContext fileUpgradeContext)
        {
            JObject projectJsonObject = fileUpgradeContext.ProjectJsonObject;
            JObject dependencies = (JObject)projectJsonObject["dependencies"];

            var dependenciesToUpdate = new List<JProperty>();
            foreach (var dependencyProp in dependencies.Properties())
            {
                if (_dependencyFilter(ReleaseVersion,dependencyProp))
                {
                    dependenciesToUpdate.Add(dependencyProp);
                }
            }

            foreach (var dependencyForUpdate in dependenciesToUpdate)
            {
                _updateDependencyCallback(ReleaseVersion,dependencies, dependencyForUpdate);
            }
        }

        public ReleaseVersion ReleaseVersion { get; set; }

    }
}