using System.Collections.Generic;
using AspNetUpgrade.Upgrader;

namespace AspNetUpgrade.Model
{
    public static class PackageMigrationHelper
    {

        public static List<DependencyPackageMigrationInfo> GetRc2DependencyPackageMigrationList(ToolingVersion targetToolingVersion)
        {
            var list = new List<DependencyPackageMigrationInfo>();
            string toolingVersion = ToolingVersion.Preview1.ToString().ToLowerInvariant();

            // ef packages..
            var package = new DependencyPackageMigrationInfo("Microsoft.EntityFrameworkCore.SqlServer", "1.0.0-rc2-final");
            package.OldNames.Add("EntityFramework.MicrosoftSqlServer");
            package.OldNames.Add("EntityFramework.SqlServer");
            list.Add(package);

            package = new DependencyPackageMigrationInfo("Microsoft.EntityFrameworkCore.SQLite", "1.0.0-rc2-final");
            package.OldNames.Add("EntityFramework.SQLite");
            list.Add(package);

            //package = new NuGetPackageMigrationInfo("NpgSql.EntityFrameworkCore.Postgres", "1.0.0-rc2-final");
            //package.OldNames.Add("EntityFramework7.Npgsql");
            //list.Add(package);

            package = new DependencyPackageMigrationInfo("EntityFrameworkCore.SqlServerCompact35", "1.0.0-rc2-final");
            package.OldNames.Add("EntityFramework.SqlServerCompact35");
            list.Add(package);

            package = new DependencyPackageMigrationInfo("EntityFrameworkCore.SqlServerCompact40", "1.0.0-rc2-final");
            package.OldNames.Add("EntityFramework.SqlServerCompact40");
            list.Add(package);

            package = new DependencyPackageMigrationInfo("Microsoft.EntityFrameworkCore.InMemory", "1.0.0-rc2-final");
            package.OldNames.Add("EntityFramework.InMemory");
            list.Add(package);


            //package = new NuGetPackageMigrationInfo("EntityFramework.IBMDataServer", "1.0.0-rc2-final");
            //package.OldNames.Add("EntityFramework.IBMDataServer");
            //list.Add(package);

            package = new DependencyPackageMigrationInfo("Microsoft.EntityFrameworkCore.Tools", $"1.0.0-{toolingVersion}-final");
            package.Type = PackageType.Build;
            package.OldNames.Add("EntityFramework.Commands");
            list.Add(package);


            package = new DependencyPackageMigrationInfo("Microsoft.EntityFrameworkCore.SqlServer.Design", "1.0.0-rc2-final");
            package.OldNames.Add("EntityFramework.MicrosoftSqlServer.Design");
            list.Add(package);


            // aspnet packages
            package = new DependencyPackageMigrationInfo("Microsoft.AspNetCore.Identity.EntityFrameworkCore", "1.0.0-rc2-final");
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


            package = new DependencyPackageMigrationInfo("Microsoft.VisualStudio.Web.CodeGenerators.Mvc", $"1.0.0-{toolingVersion}-final");
            package.OldNames.Add("Microsoft.Extensions.CodeGenerators.Mvc");
            package.Type = PackageType.Build;
            list.Add(package);

            package = new DependencyPackageMigrationInfo("Microsoft.ApplicationInsights.AspNetCore", "1.0.0-rc2-final");
            package.OldNames.Add("Microsoft.ApplicationInsights.AspNet");
            list.Add(package);

            package = new DependencyPackageMigrationInfo("Microsoft.Extensions.Configuration.FileExtensions", "1.0.0-rc2-final");
            package.OldNames.Add("Microsoft.Extensions.Configuration.FileProviderExtensions");
            list.Add(package);

            // remove Microsoft.Dnx.Runtime
            package = new DependencyPackageMigrationInfo("Microsoft.Dnx.Runtime", "1.0.0-rc1-final");
            package.MigrationAction = PackageMigrationAction.Remove;
            list.Add(package);

            // Microsoft.VisualStudio.Web.BrowserLink.Loader




            return list;

        }


        public static List<ToolPackageMigrationInfo> GetRc2ToolPackageMigrationList(ToolingVersion targetToolingVersion, IJsonProjectUpgradeContext projectContext)
        {
            var list = new List<ToolPackageMigrationInfo>();
            string toolingVersion = ToolingVersion.Preview1.ToString().ToLowerInvariant();

            // Updates the old web command if it is found, to be the new tool.
            var package = new ToolPackageMigrationInfo("Microsoft.AspNetCore.Server.IISIntegration.Tools", $"1.0.0-{toolingVersion}-final");
            package.OldNames.Add("web");
            package.Imports.Add("portable-net45+win8+dnxcore50");
            package.MigrationAction = PackageMigrationAction.Update;
            list.Add(package);

            // Updates the old ef command if it is found, to be the new tool.
            package = new ToolPackageMigrationInfo("Microsoft.EntityFrameworkCore.Tools", $"1.0.0-{toolingVersion}-final");
            package.OldNames.Add("ef");
            package.Imports.Add("portable-net45+win8+dnxcore50");
            package.Imports.Add("portable-net45+win8");
            package.MigrationAction = PackageMigrationAction.Update;
            list.Add(package);

            // Adds / updates the user secrets tool - if the project.json has a userSecretsId present.
            if (projectContext.JsonObject["userSecretsId"] != null)
            {
                package = new ToolPackageMigrationInfo("Microsoft.Extensions.SecretManager.Tools", $"1.0.0-{toolingVersion}-final");
                package.Imports.Add("portable-net45+win8+dnxcore50");
                package.MigrationAction = PackageMigrationAction.AddOrUpdate;
                list.Add(package);
            }

            // only add the razor tools tool if project allready references razor tools package as a dependency (either as new or old name) in case we havent updated it yet)
            if (projectContext.ToProjectJsonWrapper().HasDependency("Microsoft.AspNetCore.Razor.Tools")
                || projectContext.ToProjectJsonWrapper().HasDependency("Microsoft.AspNet.Tooling.Razor"))
            {
                package = new ToolPackageMigrationInfo("Microsoft.AspNetCore.Razor.Tools", $"1.0.0-{toolingVersion}-final");
                package.Imports.Add("portable-net45+win8+dnxcore50");
                package.MigrationAction = PackageMigrationAction.AddOrUpdate;
                list.Add(package);
            }


            //    var projectType = projectContext.ToProjectJsonWrapper().GetProjectType();

            // only add the web code generation tools to the project if its a web project. We use a heuristic - if MVC is there as a dependency then its a web project.
            if (projectContext.ToProjectJsonWrapper().HasDependency("Microsoft.AspNetCore.Mvc")
              || projectContext.ToProjectJsonWrapper().HasDependency("Microsoft.AspNet.Mvc"))
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