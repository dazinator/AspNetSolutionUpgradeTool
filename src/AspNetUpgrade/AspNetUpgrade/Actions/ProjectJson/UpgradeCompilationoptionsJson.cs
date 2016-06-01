using AspNetUpgrade.Upgrader;
using Newtonsoft.Json.Linq;

namespace AspNetUpgrade.Actions.ProjectJson
{
    public class UpgradeCompilationOptionsJson : IProjectJsonUpgradeAction
    {

        public void Apply(IJsonFileUpgradeContext fileUpgradeContext)
        {
            JObject projectJsonObject = fileUpgradeContext.ProjectJsonObject;
            JObject compilationOptions = (JObject)projectJsonObject["compilationOptions"];
            compilationOptions.Rename("buildOptions");
        }

        public void Undo(IJsonFileUpgradeContext fileUpgradeContext)
        {
            JObject projectJsonObject = fileUpgradeContext.ProjectJsonObject;
            JObject buildOptions = (JObject)projectJsonObject["buildOptions"];
            buildOptions.Rename("compilationOptions");
        }


    }
}