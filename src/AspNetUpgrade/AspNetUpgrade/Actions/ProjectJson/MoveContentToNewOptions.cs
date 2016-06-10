using System;
using AspNetUpgrade.UpgradeContext;
using Newtonsoft.Json.Linq;

namespace AspNetUpgrade.Actions.ProjectJson
{

    public class MoveContentToNewOptions : IProjectUpgradeAction
    {

        public void Apply(IProjectUpgradeContext fileUpgradeContext)
        {
            JObject projectJsonObject = fileUpgradeContext.ProjectJsonObject;
            MoveContent(projectJsonObject);
        }

        private static void MoveContent(JObject root)
        {
            var contentItems = new[]
            {
                Tuple.Create("content", "include"),
                Tuple.Create("contentExclude", "exclude"),
                Tuple.Create("contentFiles", "includeFiles")
            };

            foreach (var item in contentItems)
            {
                var property = root.Property(item.Item1);
                if (property != null)
                {
                    var publishInclude = root.GetOrAddProperty("publishOptions", property);
                    publishInclude[item.Item2] = property.Value;

                    var buildOptions = root.GetOrAddProperty("buildOptions", property);
                    var copyToOutput = buildOptions.GetOrAddProperty("copyToOutput", null);
                    copyToOutput[item.Item2] = property.Value;

                    property.Remove();
                }
            }
        }
    }

}