using Newtonsoft.Json.Linq;

namespace AspNetUpgrade.Actions
{
    //public enum TargetFrameworkKind
    //{
    //    DotNetCore,
    //    Desktop
    //}


    public class UpgradeFrameworksJson : IAction
    {

        private JProperty BuildNetCoreAppFrameworkJson()
        {

            JArray importsArray = new JArray();
            importsArray.Add("dotnet5.6");
            importsArray.Add("dnxcore50");
            importsArray.Add("portable-net45+win8");

            var importsObj = new JObject(new JProperty("imports", importsArray));
            //importsObj.Add(importsArray);
            return new JProperty("netcoreapp1.0", importsObj);

            //netcoreapp1.Add(importsObj);

            // return netcoreapp1;

        }

        private JToken _oldFrameworks;

        public void Apply(IJsonFileUpgradeContext fileUpgradeContext)
        {

            JObject projectJsonObject = fileUpgradeContext.ProjectJsonObject;
            JObject frameworks = (JObject)projectJsonObject["frameworks"];
            _oldFrameworks = frameworks.DeepClone();

            // rename dnx451
            var dnx451 = frameworks.Property("dnx451");
            if (dnx451 != null)
            {
                var renamed = dnx451.Rename(name => name == "dnx451" ? "net451" : name);
                dnx451.Replace(renamed);
            }

            // rename dnx452
            var dnx452 = frameworks.Property("dnx452");
            if (dnx452 != null)
            {
                var renamed = dnx452.Rename(name => name == "dnx452" ? "net452" : name);
                dnx452.Replace(renamed);
            }

            var dnxCore50 = frameworks.Property("dnxcore50");
            if (dnxCore50 != null)
            {
                dnxCore50.Remove();
                var netCoreObject = BuildNetCoreAppFrameworkJson();
                frameworks.Add(netCoreObject);
            }

        }

        public void Undo(IJsonFileUpgradeContext fileUpgradeContext)
        {
            // restore frameworks section
            JObject projectJsonObject = fileUpgradeContext.ProjectJsonObject;
            projectJsonObject["frameworks"].Replace(_oldFrameworks);

        }
    }
}
