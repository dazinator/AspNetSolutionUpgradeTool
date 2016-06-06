using AspNetUpgrade.Model;
using AspNetUpgrade.UpgradeContext;
using Newtonsoft.Json.Linq;

namespace AspNetUpgrade.Actions.ProjectJson
{

    public class UpgradePublishOptions : IProjectUpgradeAction
    {
        public void Apply(IProjectUpgradeContext fileUpgradeContext)
        {
            JObject projectJsonObject = fileUpgradeContext.JsonObject;

            JArray exclude = (JArray)projectJsonObject["exclude"];
            JArray publishExclude = (JArray)projectJsonObject["publishExclude"];

            projectJsonObject.Remove("exclude");
            projectJsonObject.Remove("publishExclude");

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
                //                    "appsettings.json",
                //                    "web.config"
                //]
                //  },

            }
        }
    }

}