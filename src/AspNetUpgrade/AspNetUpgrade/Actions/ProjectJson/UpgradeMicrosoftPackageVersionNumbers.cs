using System.Linq;
using AspNetUpgrade.Migrator;
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


        public UpgradeMicrosoftPackageVersionNumbers(ReleaseVersion version) : base(version, DependencyPredicate, UpdateDependencyCallback)
        {

        }

        private static void UpdateDependencyCallback(ReleaseVersion version, JObject dependencies, JProperty dependencyProperty)
        {

            string browserLinkloaderVersion;
            string newVersion;

            if (version == ReleaseVersion.RTM)
            {
                // update to RTM version numbers
                newVersion = "1.0.0";
                browserLinkloaderVersion = "14.0.0";
            }
            else
            {
                // update to RC2 version numbers
                newVersion = "1.0.0-rc2-final";
                browserLinkloaderVersion = "14.0.0-rc2-final";
            }


            switch (dependencyProperty.Name.ToLowerInvariant())
            {
                case "microsoft.visualstudio.web.browserlink.loader":
                    dependencies[dependencyProperty.Name] = browserLinkloaderVersion;
                    break;
                default:
                    dependencies[dependencyProperty.Name] = newVersion;
                    break;
            }


        }

        private static bool DependencyPredicate(ReleaseVersion version, JProperty property)
        {
            // detect rc1 packages for update by default, unless we are updating to RTM and then we detect RC2 packages.
            string oldPreReleaseLabel = "rc1";
            if (version == ReleaseVersion.RTM)
            {
                return !GetRC1PackageNamesToExcludeFromRenaming().Contains(property.Name) &&
                       property.Name.ToLowerInvariant().StartsWith("microsoft.")
                       &&
                       (property.Value.ToString().ToLowerInvariant().Contains(oldPreReleaseLabel) ||
                        property.Value.ToString().ToLowerInvariant().Contains("rc2"));



            }
            else if (version == ReleaseVersion.RC2)
            {
                return !GetRC1PackageNamesToExcludeFromRenaming().Contains(property.Name) && property.Name.ToLowerInvariant().StartsWith("microsoft.")
               && property.Value.ToString().ToLowerInvariant().Contains(oldPreReleaseLabel);

            }

            return false;


        }
    }
}