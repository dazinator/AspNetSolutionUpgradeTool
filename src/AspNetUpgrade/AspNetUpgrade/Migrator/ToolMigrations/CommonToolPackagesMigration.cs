using System.Collections.Generic;
using AspNetUpgrade.Model;
using AspNetUpgrade.UpgradeContext;

namespace AspNetUpgrade.Migrator.ToolMigrations
{
    public class CommonToolPackagesMigration : IToolPackageMigrationProvider
    {
        public List<ToolPackageMigrationInfo> GetPackageMigrations(ToolingVersion targetToolingVersion, IProjectUpgradeContext projectContext)
        {
            var list = new List<ToolPackageMigrationInfo>();
            string toolingVersion = targetToolingVersion.ToString().ToLowerInvariant();

            // Updates the old web command if it is found, to be the new tool.
            var package = new ToolPackageMigrationInfo("Microsoft.AspNetCore.Server.IISIntegration.Tools", $"1.0.0-{toolingVersion}-final");
            package.OldNames.Add("web");
            package.Imports.Add("portable-net45+win8+dnxcore50");
            package.MigrationAction = PackageMigrationAction.Update;
            list.Add(package);


            // Adds / updates the user secrets tool - if the project.json has a userSecretsId present.
            if (projectContext.ProjectJsonObject["userSecretsId"] != null)
            {
                package = new ToolPackageMigrationInfo("Microsoft.Extensions.SecretManager.Tools", $"1.0.0-{toolingVersion}-final");
                package.Imports.Add("portable-net45+win8+dnxcore50");
                package.MigrationAction = PackageMigrationAction.AddOrUpdate;
                list.Add(package);
            }

            // Updates the old ef command if it is found, to be the new tool.
            package = new ToolPackageMigrationInfo("Microsoft.EntityFrameworkCore.Tools", $"1.0.0-{toolingVersion}-final");
            package.OldNames.Add("ef");
            package.Imports.Add("portable-net45+win8+dnxcore50");
            package.Imports.Add("portable-net45+win8");
            package.MigrationAction = PackageMigrationAction.Update;
            list.Add(package);


            // only add the razor tools tool if project allready references razor tools package as a dependency (either as new or old name) in case we havent updated it yet)
            if (projectContext.ToProjectJsonWrapper().HasDependency("Microsoft.AspNetCore.Razor.Tools")
                || projectContext.ToProjectJsonWrapper().HasDependency("Microsoft.AspNet.Tooling.Razor"))
            {
                package = new ToolPackageMigrationInfo("Microsoft.AspNetCore.Razor.Tools", $"1.0.0-{toolingVersion}-final");
                package.Imports.Add("portable-net45+win8+dnxcore50");
                package.MigrationAction = PackageMigrationAction.AddOrUpdate;
                list.Add(package);
            }

            // only add the web code generation tools to the project if its a web project. We use a heuristic - if MVC is there as a dependency then its a web project.
            if (projectContext.ToProjectJsonWrapper().IsMvcProject())
            {
                package = new ToolPackageMigrationInfo("Microsoft.VisualStudio.Web.CodeGeneration.Tools", $"1.0.0-{toolingVersion}-final");
                package.Imports.Add("portable-net45+win8+dnxcore50");
                package.Imports.Add("portable-net45+win8");
                package.MigrationAction = PackageMigrationAction.AddOrUpdate;
                list.Add(package);
            }


            return list;
        }
    }
}