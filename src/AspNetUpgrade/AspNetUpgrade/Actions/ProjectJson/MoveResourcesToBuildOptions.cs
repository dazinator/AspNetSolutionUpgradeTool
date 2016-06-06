using System;
using AspNetUpgrade.UpgradeContext;
using Newtonsoft.Json.Linq;

namespace AspNetUpgrade.Actions.ProjectJson
{
    public class MoveResourcesToBuildOptions : IProjectUpgradeAction
    {

        public void Apply(IProjectUpgradeContext fileUpgradeContext)
        {
            JObject projectJsonObject = fileUpgradeContext.JsonObject;
            MoveResourcesToBuild(projectJsonObject);
        }

        private static void MoveResourcesToBuild(JObject root)
        {
            var resourceItems = new[]
            {
                Tuple.Create("resource", "include"),
                Tuple.Create("namedResource", "mappings"),
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
                    property.Remove();
                }
            }
        }
    }

}