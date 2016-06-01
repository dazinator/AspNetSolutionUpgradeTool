using AspNetUpgrade.Upgrader;
using Newtonsoft.Json.Linq;

namespace AspNetUpgrade.Actions.ProjectJson
{
    public class AddRuntimeDependency : IProjectJsonUpgradeAction
    {

        private JToken _backup;

        private JProperty BuildNetCoreAppDependency(string netCoreAppVersion)
        {

            JObject netCoreAppObject = new JObject();
            netCoreAppObject.Add(new JProperty("version", netCoreAppVersion));
            netCoreAppObject.Add(new JProperty("type", "platform"));

            var netCoreAooProperty = new JProperty("Microsoft.NETCore.App", netCoreAppObject);
            return netCoreAooProperty;

        }

        public void Apply(IJsonFileUpgradeContext fileUpgradeContext)
        {
            JObject projectJsonObject = fileUpgradeContext.ProjectJsonObject;
            JObject dependencies = (JObject)projectJsonObject["dependencies"];
            _backup = dependencies.DeepClone();

            // add Microsoft.NETCore.App
            var netCoreAppDependency = BuildNetCoreAppDependency("1.0.0-rc2-3002702");
            dependencies.Add(netCoreAppDependency);
        }

        public void Undo(IJsonFileUpgradeContext fileUpgradeContext)
        {
            // restore frameworks section
            JObject projectJsonObject = fileUpgradeContext.ProjectJsonObject;
            projectJsonObject["dependencies"].Replace(_backup);

        }
    }
}