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
            var projectJsonWrapper = fileUpgradeContext.ToProjectJsonWrapper();
            if (projectJsonWrapper.IsMvcProject())
            {
                var items = new JArray();
                items.Add("Views/**");

                projectJsonWrapper.IncludeInCopyToOutput(items);
               
            }

        }

    }

}