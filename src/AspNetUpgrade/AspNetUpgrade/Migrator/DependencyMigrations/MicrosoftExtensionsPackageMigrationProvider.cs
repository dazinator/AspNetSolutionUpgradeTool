using System.Collections.Generic;
using AspNetUpgrade.Model;
using AspNetUpgrade.UpgradeContext;

namespace AspNetUpgrade.Migrator.DependencyMigrations
{
    public class MicrosoftExtensionsDependencyPackageMigrationProvider : IDependencyPackageMigrationProvider
    {
        public List<DependencyPackageMigrationInfo> GetPackageMigrations(ToolingVersion targetToolingVersion, IProjectUpgradeContext projectContext)
        {
            var list = new List<DependencyPackageMigrationInfo>();
            string toolingVersion = targetToolingVersion.ToString().ToLowerInvariant();

            var package = new DependencyPackageMigrationInfo("Microsoft.Extensions.Configuration.FileExtensions", "1.0.0-rc2-final");
            package.OldNames.Add("Microsoft.Extensions.Configuration.FileProviderExtensions");
            list.Add(package);


            // If Microsoft.Extensions.PlatformAbstractions package is referenced, it was split out in RC2 so add the new package as well, in case you were using classes from it.
            if (projectContext.ToProjectJsonWrapper().HasDependency("Microsoft.Extensions.PlatformAbstractions"))
            {
                package = new DependencyPackageMigrationInfo("Microsoft.Extensions.DependencyModel", "1.0.0-rc2-final");
                package.MigrationAction = PackageMigrationAction.AddOrUpdate;
                list.Add(package);
            }

            // only add the following new nuget packlages if the project is a web project. We use a heuristic - if MVC is there as a dependency then its a web project.
            if (projectContext.ToProjectJsonWrapper().IsMvcProject())
            {
                package = new DependencyPackageMigrationInfo("Microsoft.Extensions.Options.ConfigurationExtensions", $"1.0.0-rc2-final");
                package.MigrationAction = PackageMigrationAction.AddOrUpdate;
                list.Add(package);
            }

           

            return list;
        }
    }
}