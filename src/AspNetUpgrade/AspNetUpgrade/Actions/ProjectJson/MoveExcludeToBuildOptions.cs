using System;
using AspNetUpgrade.UpgradeContext;
using Newtonsoft.Json.Linq;

namespace AspNetUpgrade.Actions.ProjectJson
{
    public class MoveExcludeToBuildOptions : IProjectUpgradeAction
    {

        public void Apply(IProjectUpgradeContext fileUpgradeContext)
        {
            JObject projectJsonObject = fileUpgradeContext.ProjectJsonObject;
            MoveExcludeToBuild(projectJsonObject);
        }

        private static void MoveExcludeToBuild(JObject root)
        {
            var resourceItems = new[]
            {
                Tuple.Create("exclude", "exclude"),
            };

            foreach (var item in resourceItems)
            {
                var property = root.Property(item.Item1);
                if (property != null)
                {
                    var build = root.GetOrAddProperty("buildOptions", property);

                    var resource = (JObject)build["embed"];
                    if (resource == null)
                    {
                        resource = new JObject();
                        build["embed"] = resource;
                    }

                    resource[item.Item2] = property.Value;
                  
                    var compile = (JObject)build["compile"];
                    if (compile == null)
                    {
                        compile = new JObject();
                        build["compile"] = compile;
                    }

                    compile[item.Item2] = property.Value;
                    property.Remove();
                }
            }
        }
    }
}