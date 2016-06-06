using AspNetUpgrade;
using AspNetUpgrade.Upgrader;
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

        private JProperty BuildNetStandardJson(IJsonProjectUpgradeContext fileUpgradeContext)
        {
            // think these imports may only be needed temporarily for RC2 and may be disappearing after RC2?
            // applications should depend upon netcoreapp1.0
            JArray importsArray = new JArray();
            importsArray.Add("dotnet5.6");
            importsArray.Add("dnxcore50");
            importsArray.Add("portable-net45+win8");

            var importsProperty = new JProperty("imports", importsArray);
            var netCoreAppObj = new JObject(importsProperty);
            // var tfmName = "netcoreapp1.0";

            // Add the netCoreAppDependency dependency.
            // as per: https://github.com/dotnet/cli/issues/3171
            JObject netCoreAppDependencyObject = new JObject();
            netCoreAppDependencyObject.Add(new JProperty("version", _netCoreAppVersion));
            netCoreAppDependencyObject.Add(new JProperty("type", "platform"));
            // JProperty netCoreAppProperty = new JProperty("Microsoft.NETCore.App", netCoreAppObject);

            var deps = netCoreAppObj.GetOrAddProperty("dependencies", importsProperty);
            deps["Microsoft.NETCore.App"] = netCoreAppDependencyObject;

            return new JProperty(_netCoreAppTfmName, netCoreAppObj);

            //switch (projType)
            //{
            //    case ProjectType.Application:
            //        tfmName = "netcoreapp1.0";
            //        break;
            //    case ProjectType.Library:
            //        tfmName = "netstandard1.5";
            //        break;
            //    default:
            //        throw new NotSupportedException("Unsupported projecttype " + projType.ToString());
            //}

            //importsObj.Add(importsArray);


        }


        public void Apply(IJsonProjectUpgradeContext fileUpgradeContext)
        {
            JObject projectJsonObject = fileUpgradeContext.JsonObject;
            JObject frameworks = (JObject)projectJsonObject["frameworks"];

            var projType = fileUpgradeContext.ToProjectJsonWrapper().GetProjectType();
            if (projType == ProjectType.Application)
            {
                var prop = BuildNetStandardJson(fileUpgradeContext);
                frameworks.Add(prop);

            }
        }
    }
}