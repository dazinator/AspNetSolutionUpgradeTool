using AspNetUpgrade.Upgrader;
using Newtonsoft.Json.Linq;

namespace AspNetUpgrade.Actions.ProjectJson
{

    public class AddRuntimeDependency : IJsonUpgradeAction
    {

       // private JToken _backup;

        private JProperty BuildNetCoreAppDependency(ProjectType projectType, string netCoreAppVersion, string netStandardLibraryVersion)
        {

            JProperty netCoreAooProperty = null;
            switch (projectType)
            {
                case ProjectType.Application:
                    JObject netCoreAppObject = new JObject();
                    netCoreAppObject.Add(new JProperty("version", netCoreAppVersion));
                    netCoreAppObject.Add(new JProperty("type", "platform"));

                    netCoreAooProperty = new JProperty("Microsoft.NETCore.App", netCoreAppObject);

                    break;
                case ProjectType.Library:
                    netCoreAooProperty = new JProperty("NETStandard.Library", netStandardLibraryVersion);
                    break;
            }

            return netCoreAooProperty;

        }
    

        public void Apply(IJsonProjectUpgradeContext fileUpgradeContext)
        {
            JObject projectJsonObject = fileUpgradeContext.JsonObject;
            JObject dependencies = (JObject)projectJsonObject["dependencies"];
          //  _backup = dependencies.DeepClone();

            // add Microsoft.NETCore.App
           
            JProperty frameworkDepProp = BuildNetCoreAppDependency(fileUpgradeContext.ToProjectJsonWrapper().GetProjectType(),
                "1.0.0-rc2-3002702", "1.5.0-rc2-24027");
          
            dependencies.Add(frameworkDepProp);
        }

        //public void Undo(IJsonProjectUpgradeContext fileUpgradeContext)
        //{
        //    // restore frameworks section
        //    JObject projectJsonObject = fileUpgradeContext.JsonObject;
        //    projectJsonObject["dependencies"].Replace(_backup);

        //}
    }
}