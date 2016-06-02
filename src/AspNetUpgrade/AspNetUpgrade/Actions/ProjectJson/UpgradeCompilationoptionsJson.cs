using AspNetUpgrade.Upgrader;
using Newtonsoft.Json.Linq;

namespace AspNetUpgrade.Actions.ProjectJson
{
    public class UpgradeCompilationOptionsJson : IJsonUpgradeAction
    {

        // private JToken _backup;

        public void Apply(IJsonProjectUpgradeContext fileUpgradeContext)
        {
            JObject projectJsonObject = fileUpgradeContext.JsonObject;
            JObject compilationOptions = (JObject)projectJsonObject["compilationOptions"];
            if (compilationOptions == null)
            {
                compilationOptions = new JObject();
                projectJsonObject["compilationOptions"] = compilationOptions;
            }

            //_backup = compilationOptions.DeepClone();

            compilationOptions.Rename("buildOptions");
            projectJsonObject["buildOptions"]["preserveCompilationContext"] = true;
            //compilationOptions.Add("preserveCompilationContext", true);
        }

        //public void Undo(IJsonProjectUpgradeContext fileUpgradeContext)
        //{
        //    JObject projectJsonObject = fileUpgradeContext.JsonObject;
        //    JObject buildOptions = (JObject)projectJsonObject["buildOptions"];
        //    buildOptions.Rename("compilationOptions");
        //    fileUpgradeContext.JsonObject["compilationOptions"].Replace(_backup);

        //}


    }
}