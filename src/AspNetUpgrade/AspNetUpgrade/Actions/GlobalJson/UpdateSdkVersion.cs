using AspNetUpgrade.Actions.ProjectJson;
using AspNetUpgrade.Upgrader;
using Newtonsoft.Json.Linq;

namespace AspNetUpgrade.Actions.GlobalJson
{
    public class UpdateSdkVersion : IJsonUpgradeAction
    {

        private JToken _backup;

        private string _sdkVersion;

        public UpdateSdkVersion(string sdkVersion)
        {
            _sdkVersion = sdkVersion;
        }

        public void Apply(IJsonProjectUpgradeContext fileUpgradeContext)
        {
            JObject jsonObject = fileUpgradeContext.JsonObject;
            JObject sdk = (JObject)jsonObject["sdk"];
            if (sdk != null)
            {
                _backup = sdk.DeepClone();
                var versionprop = sdk.Property("version");
                if (versionprop != null && versionprop.Value != null)
                {
                    sdk["version"] = _sdkVersion;
                }

            }
        }

        public void Undo(IJsonProjectUpgradeContext fileUpgradeContext)
        {
            // restore frameworks section
            JObject projectJsonObject = fileUpgradeContext.JsonObject;
            projectJsonObject["sdk"].Replace(_backup);

        }
    }
}