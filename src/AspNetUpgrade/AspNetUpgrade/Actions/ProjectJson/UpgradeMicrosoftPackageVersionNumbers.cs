using System.Linq;
using Newtonsoft.Json.Linq;

namespace AspNetUpgrade.Actions.ProjectJson
{
    public class UpgradeMicrosoftPackageVersionNumbers : BaseDependenciesUpdate
    {

        private static string[] packagesToExclude = new[] { "Microsoft.AspNet.WebApi.Client" };

        /// <summary>
        /// Returns a list of RC1 nuget package names that should not be updated to version "1.0.0-rc2-final" when updating all
        /// microsoft packages that have rc1 in the version.
        /// </summary>
        /// <returns></returns>
        public static string[] GetRC1PackageNamesToExcludeFromRenaming()
        {
            return packagesToExclude;
        }


        public UpgradeMicrosoftPackageVersionNumbers() : base(DependencyPredicate, UpdateDependencyCallback)
        {


        }

        private static void UpdateDependencyCallback(JObject dependencies, JProperty dependencyProperty)
        {
            switch (dependencyProperty.Name.ToLowerInvariant())
            {
                case "microsoft.visualstudio.web.browserlink.loader":
                    dependencies[dependencyProperty.Name] = "14.0.0-rc2-final";
                    break;
                default:
                    dependencies[dependencyProperty.Name] = "1.0.0-rc2-final";
                    break;
            }

        }

        private static bool DependencyPredicate(JProperty property)
        {
            return !GetRC1PackageNamesToExcludeFromRenaming().Contains(property.Name) && property.Name.ToLowerInvariant().StartsWith("microsoft.") && property.Value.ToString().ToLowerInvariant().Contains("rc1");
        }
    }
}