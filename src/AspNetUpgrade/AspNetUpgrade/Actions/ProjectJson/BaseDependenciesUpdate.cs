using System;
using System.Collections.Generic;
using AspNetUpgrade.Model;
using AspNetUpgrade.Upgrader;
using Newtonsoft.Json.Linq;

namespace AspNetUpgrade.Actions.ProjectJson
{
    public abstract class BaseDependenciesUpdate : IJsonUpgradeAction
    {

      //  private JToken _backup;

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

            //  _packagePrefixToUpgrade = packageNameStartsWith;
            // _oldVersionLabelContainsThis = packageVersionNumberContains;
            //  _excludePackageNames = excludePackageNames ?? new[] { "" };
            //  _newVersionNumber = newVersionNumber;
        }

        public void Apply(IJsonProjectUpgradeContext fileUpgradeContext)
        {
            JObject projectJsonObject = fileUpgradeContext.JsonObject;
            JObject dependencies = (JObject)projectJsonObject["dependencies"];
           // _backup = dependencies.DeepClone();

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

        //public void Undo(IJsonProjectUpgradeContext fileUpgradeContext)
        //{
        //    // restore frameworks section
        //    JObject projectJsonObject = fileUpgradeContext.JsonObject;
        //    projectJsonObject["dependencies"].Replace(_backup);

        //}
    }
}