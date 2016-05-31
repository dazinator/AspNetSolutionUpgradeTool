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

        private JObject BuildNetCoreAppFrameworkJson()
        {

            JArray importsArray = new JArray();
            importsArray.Add(new JObject("dotnet5.6"));
            importsArray.Add(new JObject("dnxcore50"));
            importsArray.Add(new JObject("portable-net45+win8"));

            var importsObj = new JObject("imports");
            importsObj.Add(importsArray);

            var netcoreapp1 = new JObject("netcoreapp1.0");
            netcoreapp1.Add(importsObj);

            return netcoreapp1;

        }

        private JToken _oldFrameworks;

        public void Apply(IJsonFileUpgradeContext fileUpgradeContext)
        {

            JObject projectJsonObject = fileUpgradeContext.ProjectJsonObject;
            JObject frameworks = (JObject)projectJsonObject["frameworks"];
            _oldFrameworks = frameworks.DeepClone();

            // remove dnx451
            var dnx451 = frameworks.Property("dnx451");
            if (dnx451 != null)
            {
                dnx451.Remove();
                frameworks.Add(new JProperty("net452", new JObject()));
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

    public interface IAction
    {
        void Apply(IJsonFileUpgradeContext fileUpgradeContext);

        void Undo(IJsonFileUpgradeContext fileUpgradeContext);

    }
}
