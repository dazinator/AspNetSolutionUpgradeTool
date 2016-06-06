using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using AspNetUpgrade.Actions;
using AspNetUpgrade.Actions.ProjectJson;
using AspNetUpgrade.Actions.Xproj;
using AspNetUpgrade.Model;

namespace AspNetUpgrade.Upgrader
{

    

    public class ProjectMigrator : BaseProjectMigrator
    {

        public  ProjectMigrator(JsonProjectUpgradeContext context) : base(context)
        {
        }

        /// <summary>
        /// Migrates the project.
        /// </summary>
        /// <param name="options">migration options.</param>
        /// <param name="additionalMigrations">any additional migrations to apply.</param>
        public void Apply(MigrationOptions options, IList<IJsonUpgradeAction> additionalMigrations = null)
        {
            List<IJsonUpgradeAction> migrations = new List<IJsonUpgradeAction>();
            var context = this.Context;
            if (options.UpgradeProjectFilesToPreview1)
            {
                migrations.AddRange(GetSchemaUpgrades(context));
            }
            if (options.UpgradePackagesToRC2)
            {
                migrations.AddRange(GetPackagesUpgrades(context));
            }
            if (options.AddNetCoreTargetToApplications)
            {
                //// Makes applications target .netcoreapp.
                var addNetCoreApp = new AddNetCoreFrameworkToApplications("1.0.0-rc2-3002702");
                migrations.Add(addNetCoreApp);
            }
            if (options.AddNetStandardTargetToLibraries)
            {
                // Adds the netStandard framework, with specified version of NETStandard.Library dependency, to any library project.json's.
                var addNetStandard = new AddNetStandardFrameworkToLibrariesJson("netstandard1.5", "1.5.0-rc2-24027");
                migrations.Add(addNetStandard);
            }

            // additional migrations to applu
            if (additionalMigrations != null && additionalMigrations.Any())
            {
                migrations.AddRange(additionalMigrations);
            }

            this.Apply(migrations);
        }

        protected virtual IList<IJsonUpgradeAction> GetSchemaUpgrades(IJsonProjectUpgradeContext projectUpgradeContext)
        {
            var upgradeActions = new List<IJsonUpgradeAction>();

            // upgrades the compilation options section.
            var compilationOptionsUpgradeAction = new UpgradeCompilationOptionsJson();
            upgradeActions.Add(compilationOptionsUpgradeAction);

            // moves things to packOptions.
            var upgradePackOptions = new UpgradePackOptions();
            upgradeActions.Add(upgradePackOptions);

            // moves content to the new packOptions and buildOptions / copyToOutput
            var moveContent = new MoveContentToNewOptions();
            upgradeActions.Add(moveContent);

            // moves resources to buildOptions.
            var moveResources = new MoveResourcesToBuildOptions();
            upgradeActions.Add(moveResources);

            // move compile to build options.
            var moveCompileToBuildOptions = new MoveCompileToBuildOptions();
            upgradeActions.Add(moveCompileToBuildOptions);

            // move exclude to build options
            var moveExcludeToBuildOptions = new MoveExcludeToBuildOptions();
            upgradeActions.Add(moveExcludeToBuildOptions);

            // moves things to publishOptions.
            var upgradePublishOptions = new UpgradePublishOptions();
            upgradeActions.Add(upgradePublishOptions);

            // renames the old dnx4YZ TFM's to be the net4YZ Tfm's. 
            var frameworksUpgradeAction = new MigrateDnxFrameworksToNetFrameworksJson();
            upgradeActions.Add(frameworksUpgradeAction);

            // upgrades xproj file, by updating old dnx imports to new dotnet ones.
            var xprojImportsUpgrade = new MigrateProjectImportsFromDnxToDotNet();
            upgradeActions.Add(xprojImportsUpgrade);
            

            return upgradeActions;

        }

        protected virtual IList<IJsonUpgradeAction> GetPackagesUpgrades(IJsonProjectUpgradeContext projectUpgradeContext)
        {
            var upgradeActions = new List<IJsonUpgradeAction>();

            // migrates specific nuget packages where their name has completely changed, and also adds new ones that the project may require.
            // this is currently described by a hardcoded list.
            var nugetPackagesToMigrate = ProjectMigrator.GetRc2DependencyPackageMigrationList(ToolingVersion.Preview1, projectUpgradeContext);
            var packageMigrationAction = new MigrateDependencyPackages(nugetPackagesToMigrate);
            upgradeActions.Add(packageMigrationAction);

            // renames microsoft.aspnet packages to be microsoft.aspnetcore.
            var renameAspNetPackagesAction = new RenameAspNetPackagesToAspNetCore();
            upgradeActions.Add(renameAspNetPackagesAction);

            // updates microsoft. packages to be rc2 version numbers.
            var updateMicrosoftPackageVersionNumbersAction = new UpgradeMicrosoftPackageVersionNumbers();
            upgradeActions.Add(updateMicrosoftPackageVersionNumbersAction);

            // renames "commands" to "tools"
            var renameCommandstoToolsAndClear = new RenameCommandsToTools();
            upgradeActions.Add(renameCommandstoToolsAndClear);

            // migrates old command packages to the new tool nuget packages.
            var toolPackagestoMigrate = GetRc2ToolPackageMigrationList(ToolingVersion.Preview1, projectUpgradeContext);
            var migrateToolPackages = new MigrateToolPackages(toolPackagestoMigrate);
            upgradeActions.Add(migrateToolPackages);

            return upgradeActions;

        }

        public static List<DependencyPackageMigrationInfo> GetRc2DependencyPackageMigrationList(ToolingVersion targetToolingVersion, IJsonProjectUpgradeContext projectContext)
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

            // only add the following new nuget packlages if the project is a web project. We use a heuristic - if MVC is there as a dependency then its a web project.
            if (projectContext.ToProjectJsonWrapper().IsMvcProject())
            {
                package = new DependencyPackageMigrationInfo("Microsoft.VisualStudio.Web.CodeGeneration.Tools", $"1.0.0-{toolingVersion}-final");
                package.Type = PackageType.Build;
                package.MigrationAction = PackageMigrationAction.AddOrUpdate;
                list.Add(package);

                package = new DependencyPackageMigrationInfo("Microsoft.Extensions.Options.ConfigurationExtensions", $"1.0.0-rc2-final");
                package.MigrationAction = PackageMigrationAction.AddOrUpdate;
                list.Add(package);
            }

            // If Microsoft.Extensions.PlatformAbstractions package is referenced, it was split out in RC2 so add the new package as well, in case you were using classes from it.
            if (projectContext.ToProjectJsonWrapper().HasDependency("Microsoft.Extensions.PlatformAbstractions"))
            {
                package = new DependencyPackageMigrationInfo("Microsoft.Extensions.DependencyModel", "1.0.0-rc2-final");
                package.MigrationAction = PackageMigrationAction.AddOrUpdate;
                list.Add(package);
            }


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