using AspNetUpgrade.Model;
using AspNetUpgrade.UpgradeContext;
using Newtonsoft.Json.Linq;

namespace AspNetUpgrade.Actions.ProjectJson
{
    /// <summary>
    /// Ensures any applications target .NET Core, by adding .NET Core to the frameworks.
    /// </summary>
    public class AddNetCoreFrameworkToApplications : IProjectUpgradeAction
    {

        private const string _netCoreAppTfmName = "netcoreapp1.0";
        private string _netCoreAppVersion;

        public AddNetCoreFrameworkToApplications(string netCoreAppVersion)
        {
            _netCoreAppVersion = netCoreAppVersion;
        }

        public AddNetCoreFrameworkToApplications()
        {

        }

        public void Apply(IProjectUpgradeContext fileUpgradeContext)
        {
            JObject projectJsonObject = fileUpgradeContext.ProjectJsonObject;
            JObject frameworks = (JObject)projectJsonObject["frameworks"];

            var projWrapper = fileUpgradeContext.ToProjectJsonWrapper();
            var projType = projWrapper.GetProjectType();
            if (projType == ProjectType.Application)
            {
                if (!projWrapper.HasFramework(_netCoreAppTfmName))
                {
                    projWrapper.AddNetCoreAppFramework(_netCoreAppVersion, _netCoreAppTfmName);
                }
              
            }
        }

    }
}