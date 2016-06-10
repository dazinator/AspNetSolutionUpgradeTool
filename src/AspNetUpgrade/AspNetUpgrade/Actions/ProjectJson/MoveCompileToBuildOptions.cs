using System;
using AspNetUpgrade.UpgradeContext;
using Newtonsoft.Json.Linq;

namespace AspNetUpgrade.Actions.ProjectJson
{

    public class MoveCompileToBuildOptions : IProjectUpgradeAction
    {

        public void Apply(IProjectUpgradeContext fileUpgradeContext)
        {
            JObject projectJsonObject = fileUpgradeContext.ProjectJsonObject;
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