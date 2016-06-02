using System;
using System.Linq;
using AspNetUpgrade.Upgrader;
using Newtonsoft.Json.Linq;

namespace AspNetUpgrade.Actions.ProjectJson
{

    public class RenameCommandsToTools : IJsonUpgradeAction
    {

       // private JToken _backup;

        public void Apply(IJsonProjectUpgradeContext fileUpgradeContext)
        {
            JObject projectJsonObject = fileUpgradeContext.JsonObject;
            JObject commands = (JObject)projectJsonObject["commands"];
           // _backup = commands.DeepClone();
            commands.Rename("tools");
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