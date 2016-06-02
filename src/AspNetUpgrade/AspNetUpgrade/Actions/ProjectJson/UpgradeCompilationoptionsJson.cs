using AspNetUpgrade.Upgrader;
using Newtonsoft.Json.Linq;

namespace AspNetUpgrade.Actions.ProjectJson
{
    public class UpgradeCompilationOptionsJson : IJsonUpgradeAction
    {

        public void Apply(IJsonFileUpgradeContext fileUpgradeContext)
        {
            JObject projectJsonObject = fileUpgradeContext.JsonObject;
            JObject compilationOptions = (JObject)projectJsonObject["compilationOptions"];
            compilationOptions.Rename("buildOptions");
        }

        public void Undo(IJsonFileUpgradeContext fileUpgradeContext)
        {
            JObject projectJsonObject = fileUpgradeContext.JsonObject;
            JObject buildOptions = (JObject)projectJsonObject["buildOptions"];
            buildOptions.Rename("compilationOptions");
        }


    }
}