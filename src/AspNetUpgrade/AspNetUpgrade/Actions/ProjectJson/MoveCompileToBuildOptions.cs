using System;
using AspNetUpgrade.Upgrader;
using Newtonsoft.Json.Linq;

namespace AspNetUpgrade.Actions.ProjectJson
{

    public class MoveCompileToBuildOptions : IJsonUpgradeAction
    {

        public void Apply(IJsonProjectUpgradeContext fileUpgradeContext)
        {
            JObject projectJsonObject = fileUpgradeContext.JsonObject;
            MoveCompile(projectJsonObject);
        }

        private static void MoveCompile(JObject root)
        {
            var compile = root.Property("compile");
            if (compile == null)
            {
                return;
            }
            var buildOptions = root.GetOrAddProperty("buildOptions", compile);
            var buildCompile = buildOptions.GetOrAddProperty("compile", null);
            buildCompile["include"] = compile.Value;
            compile.Remove();
        }
    }

}