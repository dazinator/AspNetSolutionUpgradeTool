using AspNetUpgrade.Upgrader;
using Newtonsoft.Json.Linq;

namespace AspNetUpgrade.Actions.ProjectJson
{

    public class UpgradePublishOptions : IJsonUpgradeAction
    {
        public void Apply(IJsonProjectUpgradeContext fileUpgradeContext)
        {
            JObject projectJsonObject = fileUpgradeContext.JsonObject;
            JObject exclude = (JObject)projectJsonObject["exclude"];
            JObject publishExclude = (JObject)projectJsonObject["publishExclude"];
            if (exclude != null)
            {
                exclude.Remove();
            }
            if (publishExclude != null)
            {
                publishExclude.Remove();
            }

            if (fileUpgradeContext.ToProjectJsonWrapper().IsMvcProject())
            {
                // add default publishing options

                JArray includeArray = new JArray();
                includeArray.Add("wwwroot");
                //includeArray.Add("Views");
                includeArray.Add("appSettings.json");

                if (fileUpgradeContext.ToProjectJsonWrapper().GetProjectType() == ProjectType.Application)
                {
                    includeArray.Add("web.config");
                }

                var publishOptions = new JObject(new JProperty("include", includeArray));
                projectJsonObject["publishOptions"] = publishOptions;
                
                //              "publishOptions": {
                //                  "include": [
                //                    "wwwroot",
                //                    "Views",
                //                    "appsettings.json",
                //                    "web.config"
                //]
                //  },

            }
        }
    }

}