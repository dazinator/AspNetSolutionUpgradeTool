using System;
using System.Linq;
using AspNetUpgrade.UpgradeContext;
using Newtonsoft.Json.Linq;

namespace AspNetUpgrade.Actions.ProjectJson
{

    /// <summary>
    /// Include
    /// </summary>
    public class IncludeViewsFolderInCopyToOutput : IProjectUpgradeAction
    {

        public void Apply(IProjectUpgradeContext fileUpgradeContext)
        {
            JObject projectJsonObject = fileUpgradeContext.ProjectJsonObject;
            
            if (fileUpgradeContext.ToProjectJsonWrapper().IsMvcProject())
            {
                var buildOptions = projectJsonObject.GetOrAddProperty("buildOptions", null);
                var copyToOutput = buildOptions.GetOrAddProperty("copyToOutput", null);
                var existing = copyToOutput.Property("include");
                if (existing == null)
                {
                    copyToOutput["include"] = new JArray();
                }


                var existingArray = copyToOutput["include"] as JArray;
                if (existingArray != null)
                {
                    // add default publishing options
                    existingArray.Add("Views");
                }
            }

        }

    }

}