using AspNetUpgrade.Upgrader;
using Newtonsoft.Json.Linq;

namespace AspNetUpgrade.Actions.ProjectJson
{

    public class UpgradePackOptions : IJsonUpgradeAction
    {

        private static readonly string[] _packOptionProps = new[] { "repository", "tags", "licenseUrl", "projectUrl" };

        public void Apply(IJsonProjectUpgradeContext fileUpgradeContext)
        {
            JObject projectJsonObject = fileUpgradeContext.JsonObject;

            foreach (var item in _packOptionProps)
            {
                MoveToPackOptions(projectJsonObject, item);
            }
        }

        private static void MoveToPackOptions(JObject root, string item)
        {
            var property = root.Property(item);
            if (property != null)
            {
                JObject packOptions = root.GetOrAddProperty("packOptions", null);
                property.Remove();
                packOptions.Add(property);
            }

            MovePackIncludeToPackOptions(root);
        }

        private static void MovePackIncludeToPackOptions(JObject root)
        {
            var packInclude = root.Property("packInclude");
            if (packInclude != null)
            {
                var packOptions = root.GetOrAddProperty("packOptions", packInclude);
                var files = packOptions.GetOrAddProperty("files", null);
                files.Add(new JProperty("mappings", packInclude.Value));
                packInclude.Remove();
            }
        }

    }

}