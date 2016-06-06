using AspNetUpgrade.UpgradeContext;
using Newtonsoft.Json.Linq;

namespace AspNetUpgrade.Actions.ProjectJson
{

    public class RenameCommandsToTools : IProjectUpgradeAction
    {
        public void Apply(IProjectUpgradeContext fileUpgradeContext)
        {
            JObject projectJsonObject = fileUpgradeContext.JsonObject;
            JObject commands = (JObject)projectJsonObject["commands"];
            if (commands != null)
            {
                commands.Rename("tools");
            }
        }
    }

}