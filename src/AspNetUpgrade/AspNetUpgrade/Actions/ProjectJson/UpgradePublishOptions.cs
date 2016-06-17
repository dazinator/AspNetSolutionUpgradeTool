using System.Linq;
using AspNetUpgrade.Model;
using AspNetUpgrade.UpgradeContext;
using Newtonsoft.Json.Linq;

namespace AspNetUpgrade.Actions.ProjectJson
{

    public class UpgradePublishOptions : IProjectUpgradeAction
    {
        public void Apply(IProjectUpgradeContext fileUpgradeContext)
        {
            JObject projectJsonObject = fileUpgradeContext.ProjectJsonObject;

            //  JArray exclude = (JArray)projectJsonObject["exclude"];
            // JArray publishExclude = (JArray)projectJsonObject["publishExclude"];

            projectJsonObject.Remove("exclude");
            projectJsonObject.Remove("publishExclude");

            JArray includeArray = new JArray();

            if (fileUpgradeContext.ToProjectJsonWrapper().IsMvcProject())
            {
                // add default publishing options
                includeArray.Add("wwwroot");
                includeArray.Add("web.config");
            }

            var appSettingsFileNames = fileUpgradeContext.JsonFiles.Where(a => a.Name().ToLowerInvariant().StartsWith("appsettings")).ToArray();
            foreach (var item in appSettingsFileNames)
            {
                includeArray.Add(item.Name());
            }

            //includeArray.Add("Views");
            //   includeArray.Add("appSettings.json");

            var publishOptions = new JObject(new JProperty("include", includeArray));
            projectJsonObject["publishOptions"] = publishOptions;

            //              "publishOptions": {
            //                  "include": [
            //                    "wwwroot",
            //                    "appsettings.json",
            //                    "web.config"
            //]
            //  },

        }
    }
}

