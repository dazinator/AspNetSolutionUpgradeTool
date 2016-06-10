using AspNetUpgrade.Model;
using AspNetUpgrade.UpgradeContext;
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
        }


        public void Apply(IProjectUpgradeContext fileUpgradeContext)
        {
            var projType = fileUpgradeContext.ToProjectJsonWrapper().GetProjectType();
            if (projType == ProjectType.Library)
            {
                AddNetStandardFramework(fileUpgradeContext);
            }
        }

        private void AddNetStandardFramework(IProjectUpgradeContext fileUpgradeContext)
        {

            JObject projectJsonObject = fileUpgradeContext.ProjectJsonObject;
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
            netStadardDependencies["NETStandard.Library"] = _netStandardLibraryVersion;

        }
    }
}