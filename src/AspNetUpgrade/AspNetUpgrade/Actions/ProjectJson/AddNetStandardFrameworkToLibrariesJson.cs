using AspNetUpgrade.Model;
using AspNetUpgrade.UpgradeContext;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace AspNetUpgrade.Actions.ProjectJson
{
    public class AddNetStandardFrameworkToLibrariesJson : IProjectUpgradeAction
    {
        private string _netStandardLibraryVersion;
        private string _netStandardTfm;

        public AddNetStandardFrameworkToLibrariesJson(string netStandardTfm, string netStandardLibraryVersion)
        {
            _netStandardLibraryVersion = netStandardLibraryVersion;
            _netStandardTfm = netStandardTfm;
        }


        public void Apply(IProjectUpgradeContext fileUpgradeContext)
        {
            var projectWrapper = fileUpgradeContext.ToProjectJsonWrapper();
            var projType = projectWrapper.GetProjectType();
            if (projType == ProjectType.Library)
            {
                projectWrapper.AddNetStandardFramework(_netStandardLibraryVersion, _netStandardTfm);
            }
        }

      
    }
}