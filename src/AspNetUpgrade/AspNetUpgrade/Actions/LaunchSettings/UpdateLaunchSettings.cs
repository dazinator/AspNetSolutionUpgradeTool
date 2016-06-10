using System.Linq;
using AspNetUpgrade.UpgradeContext;
using Newtonsoft.Json.Linq;

namespace AspNetUpgrade.Actions.LaunchSettings
{

    public class UpdateLaunchSettings : IProjectUpgradeAction
    {
        // private string _sdkVersion;

        public UpdateLaunchSettings()
        {
            //  _sdkVersion = sdkVersion;
        }

        public void Apply(IProjectUpgradeContext upgradeContext)
        {
            JObject launchSettings = upgradeContext.LaunchSettingsObject;
            if (launchSettings != null)
            {
                JObject profiles = launchSettings.GetOrAddProperty("profiles", null);
                // replace Hosting:Environment environment variable.
                foreach (var prop in profiles.Properties())
                {
                    var profileProp = (JObject)profiles[prop.Name];

                    var envVariables = profileProp["environmentVariables"];
                    var oldHosting = envVariables?["Hosting:Environment"];
                    oldHosting?.Rename("ASPNETCORE_ENVIRONMENT");

                    // remove sdk version
                    var sdkVersion = profileProp.Property("sdkVersion");
                    if (sdkVersion != null)
                    {
                        sdkVersion.Remove();
                    }

                   // profileProp.prop["sdkVersion"].Remove();
                   // sdkVersion?.Remove();
                }

                var webProfile = profiles["web"];
                if (webProfile != null)
                {
                    string projName = upgradeContext.ProjectName();
                    webProfile.Rename(projName);

                    // ensure command name is set to project
                    webProfile["commandName"] = "Project";
                    webProfile["launchBrowser"] = true;
                    //  webProfile["launchUrl"] = true;
                    //  var projectName =  .FullPath
                }

                // set application url in iisSettings?
                JObject iisSettings = launchSettings.GetOrAddProperty("iisSettings", null);
                JObject iisExpressSettings = iisSettings.GetOrAddProperty("iisExpress", null);
                iisExpressSettings["applicationUrl"] = "http://localhost:50000/";

            }

        }


    }
}