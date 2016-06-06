using AspNetUpgrade.Actions.ProjectJson;
using AspNetUpgrade.UpgradeContext;
using Newtonsoft.Json.Linq;

namespace AspNetUpgrade.Actions.GlobalJson
{

    
    public class UpdateSdkVersion : ISolutionUpgradeAction
    {
        private string _sdkVersion;

        public UpdateSdkVersion(string sdkVersion)
        {
            _sdkVersion = sdkVersion;
        }

        public void Apply(ISolutionUpgradeContext upgradeContext)
        {
            JObject jsonObject = upgradeContext.GlobalJsonObject;
            JObject sdk = (JObject)jsonObject["sdk"];
            if (sdk != null)
            {
                var versionprop = sdk.Property("version");
                if (versionprop != null && versionprop.Value != null)
                {
                    sdk["version"] = _sdkVersion;
                }

            }
        }
    }
}