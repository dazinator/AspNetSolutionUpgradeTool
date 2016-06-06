using System;
using System.Collections.Generic;
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

        private Predicate<JProperty> _dependencyFilter;
        private Action<JObject, JProperty> _updateDependencyCallback;


        protected BaseDependenciesUpdate(Predicate<JProperty> dependencyPredicate, Action<JObject, JProperty> updateDependencyCallback)
        {
            _dependencyFilter = dependencyPredicate;
            _updateDependencyCallback = updateDependencyCallback;
        }

        public void Apply(IProjectUpgradeContext fileUpgradeContext)
        {
            JObject projectJsonObject = fileUpgradeContext.JsonObject;
            JObject dependencies = (JObject)projectJsonObject["dependencies"];

            var dependenciesToUpdate = new List<JProperty>();
            foreach (var dependencyProp in dependencies.Properties())
            {
                if (_dependencyFilter(dependencyProp))
                {
                    dependenciesToUpdate.Add(dependencyProp);
                }
            }

            foreach (var dependencyForUpdate in dependenciesToUpdate)
            {
                _updateDependencyCallback(dependencies, dependencyForUpdate);
            }
        }
    }
}