using System.Collections.Generic;
using AspNetUpgrade.Model;
using AspNetUpgrade.UpgradeContext;

namespace AspNetUpgrade.Migrator.DependencyMigrations
{
    public class AspNetDependencyPackageMigrationProvider : IDependencyPackageMigrationProvider
    {
        public List<DependencyPackageMigrationInfo> GetPackageMigrations(ToolingVersion targetToolingVersion, IProjectUpgradeContext projectContext)
        {
            var list = new List<DependencyPackageMigrationInfo>();
            string toolingVersion = ToolingVersion.Preview1.ToString().ToLowerInvariant();

            // aspnet packages
            var package = new DependencyPackageMigrationInfo("Microsoft.AspNetCore.Identity.EntityFrameworkCore", "1.0.0-rc2-final");
            package.OldNames.Add("Microsoft.AspNet.Identity.EntityFramework");
            list.Add(package);

            package = new DependencyPackageMigrationInfo("Microsoft.AspNetCore.Server.IISIntegration", "1.0.0-rc2-final");
            package.OldNames.Add("Microsoft.AspNet.IISPlatformHandler");
            list.Add(package);

            package = new DependencyPackageMigrationInfo("Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore", "1.0.0-rc2-final");
            package.OldNames.Add("Microsoft.AspNet.Diagnostics.Entity");
            list.Add(package);


            package = new DependencyPackageMigrationInfo("Microsoft.AspNetCore.Razor.Tools", $"1.0.0-{toolingVersion}-final");
            package.Type = PackageType.Build;
            package.OldNames.Add("Microsoft.AspNet.Tooling.Razor");
            list.Add(package);

            var projectWrapper = projectContext.ToProjectJsonWrapper();
            // only add the following new nuget packlages if the project is a web project. We use a heuristic - if MVC is there as a dependency then its a web project.
            if (projectWrapper.IsMvcProject())
            {
                package = new DependencyPackageMigrationInfo("Microsoft.VisualStudio.Web.CodeGeneration.Tools", $"1.0.0-{toolingVersion}-final");
                package.Type = PackageType.Build;
                package.MigrationAction = PackageMigrationAction.AddOrUpdate;
                list.Add(package);

                // ensure Microsoft.AspNetCore.Diagnostics is brought in
                package = new DependencyPackageMigrationInfo("Microsoft.AspNetCore.Diagnostics", "1.0.0-rc2-final");
                package.MigrationAction = PackageMigrationAction.AddOrUpdate;
                list.Add(package);
            }

            package = new DependencyPackageMigrationInfo("Microsoft.VisualStudio.Web.CodeGenerators.Mvc", $"1.0.0-{toolingVersion}-final");
            package.OldNames.Add("Microsoft.Extensions.CodeGenerators.Mvc");
            package.Type = PackageType.Build;
            list.Add(package);


            package = new DependencyPackageMigrationInfo("Microsoft.ApplicationInsights.AspNetCore", "1.0.0-rc2-final");
            package.OldNames.Add("Microsoft.ApplicationInsights.AspNet");
            list.Add(package);

            // Ensure following package is present if depending on IISPlatformHandler
            if (projectWrapper.HasDependency("Microsoft.AspNet.IISPlatformHandler") ||
                projectWrapper.HasDependency("Microsoft.AspNetCore.Server.IISIntegration"))
            {
                package = new DependencyPackageMigrationInfo("Microsoft.AspNetCore.Hosting", $"1.0.0-rc2-final");
                package.MigrationAction = PackageMigrationAction.AddOrUpdate;
                list.Add(package);
            }

            // remove Microsoft.Dnx.Runtime
            package = new DependencyPackageMigrationInfo("Microsoft.Dnx.Runtime", "1.0.0-rc1-final");
            package.MigrationAction = PackageMigrationAction.Remove;
            list.Add(package);


            return list;
        }
    }


}