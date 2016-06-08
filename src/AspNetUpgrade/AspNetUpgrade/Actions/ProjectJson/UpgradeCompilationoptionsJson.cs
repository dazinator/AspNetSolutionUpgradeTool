using AspNetUpgrade.UpgradeContext;
using Newtonsoft.Json.Linq;

namespace AspNetUpgrade.Actions.ProjectJson
{
    public class UpgradeCompilationOptionsJson : IProjectUpgradeAction
    {

        public void Apply(IProjectUpgradeContext fileUpgradeContext)
        {
            JObject projectJsonObject = fileUpgradeContext.JsonObject;
            JObject compilationOptions = (JObject)projectJsonObject["compilationOptions"];
            if (compilationOptions != null)
            {
                compilationOptions.Rename("buildOptions");
               // compilationOptions = new JObject();
               // projectJsonObject["compilationOptions"] = compilationOptions;
            }

            var buildOptions = projectJsonObject["buildOptions"];
            if (buildOptions == null)
            {
                buildOptions = new JObject();
                projectJsonObject["buildOptions"] = buildOptions;
            }

            buildOptions["preserveCompilationContext"] = true;

            foreach (var item in ((JObject)projectJsonObject["frameworks"]).Properties())
            {
                var tfmCompilationOptions = ((JObject)item.Value).Property("compilationOptions");
                if (tfmCompilationOptions != null)
                {
                    tfmCompilationOptions.Replace(new JProperty("buildOptions", tfmCompilationOptions.Value));
                }
            }
          
        }


    }
}