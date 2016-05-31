using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace AspNetUpgrade.Actions
{
    public class UpgradeCompilationoptionsJson : IAction
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