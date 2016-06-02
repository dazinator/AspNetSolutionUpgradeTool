using System;
using System.Collections;
using System.ComponentModel;
using AspNetUpgrade.Upgrader;
using Newtonsoft.Json.Linq;

namespace AspNetUpgrade.Actions.ProjectJson
{

    public class UpgradeFrameworksJson : IJsonUpgradeAction
    {
        //private JToken _oldFrameworks;


        private JProperty BuildNetStandardJson(IJsonProjectUpgradeContext fileUpgradeContext)
        {
            // think these imports may only be needed temporarily for RC2 and may be disappearing after RC2?
            JArray importsArray = new JArray();
            importsArray.Add("dotnet5.6");
            importsArray.Add("dnxcore50");
            importsArray.Add("portable-net45+win8");

            var importsObj = new JObject(new JProperty("imports", importsArray));

            // applications should depend upon netcoreapp1.0
            string tfmName;
            var projType = fileUpgradeContext.ToProjectJsonWrapper().GetProjectType();
            switch (projType)
            {
                case ProjectType.Application:
                    tfmName = "netcoreapp1.0";
                    break;
                case ProjectType.Library:
                    tfmName = "netstandard1.5";
                    break;
                default:
                    throw new NotSupportedException("Unsupported projecttype " + projType.ToString());
            }

            //importsObj.Add(importsArray);
            return new JProperty(tfmName, importsObj);

        }


        public void Apply(IJsonProjectUpgradeContext fileUpgradeContext)
        {

            JObject projectJsonObject = fileUpgradeContext.JsonObject;
            JObject frameworks = (JObject)projectJsonObject["frameworks"];
            //_oldFrameworks = frameworks.DeepClone();

            // dnx451
            var dnx451 = frameworks.Property("dnx451");
            if (dnx451 != null)
            {
                //  dnx451.Remove();
                // consider auto migrating dnx451 to net452 - See comments on https://github.com/aspnet/Home/issues/1381
                var renamed = dnx451.Rename(name => name == "dnx451" ? "net451" : name);
                dnx451.Replace(renamed);
            }

            // dnx452
            var dnx452 = frameworks.Property("dnx452");
            if (dnx452 != null)
            {
               // dnx452.Remove();
                var renamed = dnx452.Rename(name => name == "dnx452" ? "net452" : name);
                dnx452.Replace(renamed);
            }

            //var dnxCore50 = frameworks.Property("dnxcore50");
            //if (dnxCore50 != null)
            //{
            //    dnxCore50.Remove();
            //    var netCoreObject = BuildNetCoreFrameworkDependencyJson(fileUpgradeContext);
            //    frameworks.Add(netCoreObject);
            //}

            // add netStandard1.5
            var netStandard = BuildNetStandardJson(fileUpgradeContext);
            frameworks.Add(netStandard);

        }

        //public void Undo(IJsonProjectUpgradeContext fileUpgradeContext)
        //{
        //    // restore frameworks section
        //    JObject projectJsonObject = fileUpgradeContext.JsonObject;
        //    projectJsonObject["frameworks"].Replace(_oldFrameworks);

        //}
    }
}
