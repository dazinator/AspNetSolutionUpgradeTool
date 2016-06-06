using System;
using System.Linq;
using AspNetUpgrade.UpgradeContext;
using Newtonsoft.Json.Linq;

namespace AspNetUpgrade.Actions.ProjectJson
{

    public class RenameCommandsToTools : IProjectUpgradeAction
    {

       // private JToken _backup;

        public void Apply(IProjectUpgradeContext fileUpgradeContext)
        {
            JObject projectJsonObject = fileUpgradeContext.JsonObject;
            JObject commands = (JObject)projectJsonObject["commands"];
            if (commands != null)
            {
                commands.Rename("tools");
            }
           // _backup = commands.DeepClone();
           
        }

        //public void Undo(IJsonProjectUpgradeContext fileUpgradeContext)
        //{
        //    JObject projectJsonObject = fileUpgradeContext.JsonObject;
        //    JObject tools = (JObject)projectJsonObject["tools"];
        //    tools.Rename("commands");
        //    projectJsonObject["commands"].Replace(_backup);
            
        //}
    }

}