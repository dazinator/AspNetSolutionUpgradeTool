using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace AspNetUpgrade.Actions
{
    public class ApplyExplicitPackageRenames : IAction
    {

        private JToken _backup;

        private List<NuGetPackageInfo> _targetPackages;

        public ApplyExplicitPackageRenames(List<NuGetPackageInfo> targetPackages)
        {
            _targetPackages = targetPackages;
        }

        //private Dictionary<string, KeyValuePair<string, string>> _packageRenames = new Dictionary<string, KeyValuePair<string, string>>()
        //{
        //    //{"EntityFramework.MicrosoftSqlServer", new KeyValuePair<string,string>("Microsoft.EntityFrameworkCore.SqlServer", "1.0.0-rc2-*")},
        //    //{"EntityFramework.SqlServer", new KeyValuePair<string, string>("Microsoft.EntityFrameworkCore.SqlServer", "1.0.0-rc2-*")},
        //   // {"Microsoft.AspNet.Identity.EntityFramework", new KeyValuePair<string, string>("Microsoft.AspNetCore.Identity.EntityFrameworkCore", "1.0.0-rc2-*")},
        //  //  {"Microsoft.AspNet.IISPlatformHandler", new KeyValuePair<string, string>("Microsoft.AspNetCore.Server.IIS", "1.0.0-rc2-*")},
        //   // {"Microsoft.AspNet.Diagnostics.Entity", new KeyValuePair<string, string>("Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore", "1.0.0-rc2-*")},
        //   // {"Microsoft.AspNet.Tooling.Razor", new KeyValuePair<string, string>("Microsoft.AspNetCore.Razor.Tools", "1.0.0-preview1-final")},
        //    //{"EntityFramework.Commands", new KeyValuePair<string, string>("Microsoft.EntityFrameworkCore.Tools", "1.0.0-preview1-final")},
        //  //  {"Microsoft.Extensions.CodeGenerators.Mvc", new KeyValuePair<string, string>("Microsoft.VisualStudio.Web.CodeGenerators.Mvc", "1.0.0-rc2-*")},
        //};


        public void Apply(IJsonFileUpgradeContext fileUpgradeContext)
        {
            JObject projectJsonObject = fileUpgradeContext.ProjectJsonObject;
            JObject dependencies = (JObject)projectJsonObject["dependencies"];
            _backup = dependencies.DeepClone();

            foreach (var targetPackage in _targetPackages)
            {
                foreach (var oldPackageName in targetPackage.OldNames)
                {
                    var dependency = dependencies[oldPackageName];
                    if (dependency != null)
                    {
                        dependency.Rename(targetPackage.Name);
                        dependencies[targetPackage.Name] = targetPackage.Version;
                        break;
                    }
                }

            }
        }

        public void Undo(IJsonFileUpgradeContext fileUpgradeContext)
        {
            // restore frameworks section
            JObject projectJsonObject = fileUpgradeContext.ProjectJsonObject;
            projectJsonObject["dependencies"].Replace(_backup);

        }
    }
}