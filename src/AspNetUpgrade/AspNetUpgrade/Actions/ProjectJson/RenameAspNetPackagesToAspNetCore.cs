using System;
using System.Linq;
using AspNetUpgrade.Migrator;
using Newtonsoft.Json.Linq;

namespace AspNetUpgrade.Actions.ProjectJson
{

    public class RenameAspNetPackagesToAspNetCore : BaseDependenciesUpdate
    {

        private static string[] packagesToExclude = new[] 
        { "Microsoft.AspNet.WebApi.Client",
          "Microsoft.AspNet.FileProviders.Abstractions",
          "Microsoft.AspNet.FileProviders.Composite",
          "Microsoft.AspNet.FileProviders.Embedded",
          "Microsoft.AspNet.FileProviders.Physical"
        };
        private const string newVersion = "1.0.0-rc2-final";

        /// <summary>
        /// Returns a list of RC1 nuget package names that should not be renamed when applying the Micrsoft.AspNet --> Microsoft.AspNetCore renaming convention.
        /// </summary>
        /// <returns></returns>
        public static string[] GetRC1PackageNamesToExcludeFromRenaming()
        {
            return packagesToExclude;
        }


        public RenameAspNetPackagesToAspNetCore() : base(ReleaseVersion.RC2, ShouldUpdateDependencyPredicate, UpdateDependencyCallback)
        {


        }

        private static void UpdateDependencyCallback(ReleaseVersion version, JObject dependencies, JProperty dependencyProperty)
        {
            var prop = dependencies[dependencyProperty.Name];
            var nameSegments = dependencyProperty.Name.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            if (nameSegments.Length > 1)
            {
                string newName = nameSegments[0];
                foreach (var nameSegment in nameSegments.Skip(1))
                {
                    if (nameSegment.ToLowerInvariant() == "aspnet")
                    {
                        newName = string.Format("{0}.{1}", newName, "AspNetCore");
                    }
                    else
                    {
                        newName = string.Format("{0}.{1}", newName, nameSegment);
                    }
                }
                prop.Rename(newName);
                dependencies[newName] = newVersion;
            }

        }

        private static bool ShouldUpdateDependencyPredicate(ReleaseVersion version, JProperty property)
        {
            return !GetRC1PackageNamesToExcludeFromRenaming().Contains(property.Name)
                   && property.Name.ToLowerInvariant().StartsWith("microsoft.aspnet")
                   && !property.Name.ToLowerInvariant().StartsWith("microsoft.aspnetcore");
        }
    }

}