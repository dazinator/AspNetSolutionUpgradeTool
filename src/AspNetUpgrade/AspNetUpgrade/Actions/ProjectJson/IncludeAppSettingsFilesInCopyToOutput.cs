using System;
using System.Linq;
using AspNetUpgrade.UpgradeContext;
using Newtonsoft.Json.Linq;

namespace AspNetUpgrade.Actions.ProjectJson
{

    /// <summary>
    /// Include
    /// </summary>
    public class IncludeAppSettingsFilesInCopyToOutput : IProjectUpgradeAction
    {

        public void Apply(IProjectUpgradeContext fileUpgradeContext)
        {
            JObject projectJsonObject = fileUpgradeContext.ProjectJsonObject;

            // get appsettings json files.
            var appSettingsFileNames =
                fileUpgradeContext.JsonFiles.Where(a => a.Name().ToLowerInvariant().StartsWith("appsettings")).ToArray();
            // for each one, include it in the publishOptions and the copytooutput.

            var buildOptions = projectJsonObject.GetOrAddProperty("buildOptions", null);

            JArray copyToOutputArray = new JArray();
            foreach (var appSettingsFile in appSettingsFileNames)
            {
                copyToOutputArray.Add(appSettingsFile.Name());
            }

          //  var existingCopyToOutput = buildOptions[copyToOutputArray];
            var copyToOutput = buildOptions.GetOrAddProperty("copyToOutput", null);
            copyToOutput["include"] = copyToOutputArray;

        }

    }

}