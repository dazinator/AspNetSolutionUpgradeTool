using AspNetUpgrade.Upgrader;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace AspNetUpgrade.Actions.ProjectJson
{
    public class AddNetStandardFrameworkToLibrariesJson : IProjectUpgradeAction
    {
        private string _netStandardLibraryVersion;
        private string _netStandardTfm;

        private string[] _imports = new[] { "dnxcore50", "portable-net45+win8" };


        public AddNetStandardFrameworkToLibrariesJson(string netStandardTfm, string netStandardLibraryVersion)
        {
            _netStandardLibraryVersion = netStandardLibraryVersion;
            _netStandardTfm = netStandardTfm;
            //  _netCoreAppVersion = netCoreAppVersion;
        }


        public void Apply(IJsonProjectUpgradeContext fileUpgradeContext)
        {
            var projType = fileUpgradeContext.ToProjectJsonWrapper().GetProjectType();
            if (projType == ProjectType.Library)
            {
                AddNetStandardFramework(fileUpgradeContext);
            }

            //  JProperty frameworkDepProp = new JProperty("NETStandard.Library", _netStandardLibraryVersion);
            //dependencies.Add(frameworkDepProp);
        }

        private void AddNetStandardFramework(IJsonProjectUpgradeContext fileUpgradeContext)
        {

            JObject projectJsonObject = fileUpgradeContext.JsonObject;
            var frameworks = projectJsonObject.GetOrAddProperty("frameworks", null);
            var netStadardTfm = frameworks.GetOrAddProperty(_netStandardTfm, null);
            var netStandardImports = netStadardTfm["imports"];

            JArray importsArray = new JArray();
            foreach (var import in _imports)
            {
                importsArray.Add(import);
            }

            if (netStandardImports == null)
            {
                netStadardTfm["imports"] = importsArray;
            }

            JObject netStadardDependencies = netStadardTfm.GetOrAddProperty("dependencies", null);

            //  _backup = dependencies.DeepClone();
            netStadardDependencies["NETStandard.Library"] = _netStandardLibraryVersion;

        }
    }
}