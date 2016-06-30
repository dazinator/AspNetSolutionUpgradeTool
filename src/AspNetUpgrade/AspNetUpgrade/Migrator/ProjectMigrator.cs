using System.Collections.Generic;
using System.Linq;
using AspNetUpgrade.Actions;
using AspNetUpgrade.Actions.LaunchSettings;
using AspNetUpgrade.Actions.ProjectJson;
using AspNetUpgrade.Actions.Xproj;
using AspNetUpgrade.Migrator.DependencyMigrations;
using AspNetUpgrade.Migrator.DependencyMigrations.RTM;
using AspNetUpgrade.Migrator.ToolMigrations;
using AspNetUpgrade.Model;
using AspNetUpgrade.UpgradeContext;

namespace AspNetUpgrade.Migrator
{
    public class ProjectMigrator : BaseProjectMigrator
    {

        public ProjectMigrator(BaseProjectUpgradeContext context) : base(context)
        {
        }

        /// <summary>
        /// Migrates the project.
        /// </summary>
        /// <param name="options">migration options.</param>
        /// <param name="additionalMigrations">any additional migrations to apply.</param>
        public void Apply(ProjectMigrationOptions options, IList<IProjectUpgradeAction> additionalMigrations = null)
        {
            List<IProjectUpgradeAction> migrations = new List<IProjectUpgradeAction>();
            var context = this.Context;
            //if (options.UpgradeToPreview1)
            //{
            migrations.AddRange(GetSchemaUpgrades(options, context));
            //}
            // if (options.UpgradePackagesToRc2)
            // {
            migrations.AddRange(GetPackagesUpgrades(context, options.PackagesVersion));
            // }

            if (options.AddNetCoreTargetForApplications)
            {
                //// Makes applications target .netcoreapp.
                var addNetCoreApp = new AddNetCoreFrameworkToApplications("1.0.0-rc2-3002702");
                migrations.Add(addNetCoreApp);
            }
            if (options.AddNetStandardTargetForLibraries)
            {
                // Adds the netStandard framework, with specified version of NETStandard.Library dependency, to any library project.json's.
                var addNetStandard = new AddNetStandardFrameworkToLibrariesJson("netstandard1.5", "1.5.0-rc2-24027");
                migrations.Add(addNetStandard);
            }


            // upgrade to preview 2
            //if (options.ToolingVersion)
            //{
            //    migrations.AddRange(GetSchemaUpgrades(options, context));
            //}
            //if (options.PackagesVersion == ReleaseVersion.RTM)
            //{
            //    migrations.AddRange(GetPackagesUpgrades(context));
            //}


            // additional migrations to apply
            if (additionalMigrations != null && additionalMigrations.Any())
            {
                migrations.AddRange(additionalMigrations);
            }

            this.Apply(migrations);
        }

        private IEnumerable<IProjectUpgradeAction> GetSchemaUpgrades(ProjectMigrationOptions options, BaseProjectUpgradeContext context)
        {
            var upgradeActions = new List<IProjectUpgradeAction>();

            //if (options.PackagesVersion == ReleaseVersion.)
            //{
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

            // includes appSettings.Json files in copyToOutput
            var includeAppSettingsFilesInCopyToOutput = new IncludeAppSettingsFilesInCopyToOutput();
            upgradeActions.Add(includeAppSettingsFilesInCopyToOutput);

            // includes Views folder in copyToOutput
            var includeViewsFolderInCopyToOutput = new IncludeViewsFolderInCopyToOutput();
            upgradeActions.Add(includeViewsFolderInCopyToOutput);

            // renames the old dnx4YZ TFM's to be the net4YZ Tfm's. 
            var frameworksUpgradeAction = new MigrateDnxFrameworksToNetFramework452Json();
            upgradeActions.Add(frameworksUpgradeAction);

            // upgrades xproj file, by updating old dnx imports to new dotnet ones.
            var xprojImportsUpgrade = new MigrateProjectImportsFromDnxToDotNet();
            upgradeActions.Add(xprojImportsUpgrade);

            // updates xproj baseintermediate output path
            var xprojUpdateBaseIntermediateOutputPath = new UpdateBaseIntermediateOutputPath();
            upgradeActions.Add(xprojUpdateBaseIntermediateOutputPath);

            // updates xproj targetFramework 
            var xprojUpdateTargetFramework = new SetTargetFrameworkVersion(options.TargetFrameworkVersionForXprojFile);
            upgradeActions.Add(xprojUpdateTargetFramework);

            // updates launch settings 
            var updateLaunchSettings = new UpdateLaunchSettings();
            upgradeActions.Add(updateLaunchSettings);
            // }

            //  if (options.PackagesVersion == ReleaseVersion.RTM)
            //  {

            // NONE NECESSAERY.
            //  }



            return upgradeActions;

        }


        protected virtual IList<IProjectUpgradeAction> GetPackagesUpgrades(IProjectUpgradeContext projectUpgradeContext, ReleaseVersion version)
        {
            var upgradeActions = new List<IProjectUpgradeAction>();


            //if (version == ReleaseVersion.RC2)
            //{
            // migrates specific nuget packages where their name has completely changed, and also adds new ones that the project may require.
            // this is currently described by a hardcoded list.
            ToolingVersion toolingVersion = ToolingVersion.Preview1;
            if (version == ReleaseVersion.RTM)
            {
                toolingVersion = ToolingVersion.Preview2;
            }
            var nugetPackagesToMigrate = ProjectMigrator.GetRc2DependencyPackageMigrationList(toolingVersion, projectUpgradeContext);
            var packageMigrationAction = new MigrateDependencyPackages(nugetPackagesToMigrate);
            upgradeActions.Add(packageMigrationAction);

            // renames microsoft.aspnet packages to be microsoft.aspnetcore.
            var renameAspNetPackagesAction = new RenameAspNetPackagesToAspNetCore();
            upgradeActions.Add(renameAspNetPackagesAction);

            // updates microsoft. packages to be rc2 version numbers.
            var updateMicrosoftPackageVersionNumbersAction = new UpgradeMicrosoftPackageVersionNumbers(version);
            upgradeActions.Add(updateMicrosoftPackageVersionNumbersAction);

            // renames "commands" to "tools"
            var renameCommandstoToolsAndClear = new RenameCommandsToTools();
            upgradeActions.Add(renameCommandstoToolsAndClear);

            // migrates old command packages to the new tool nuget packages.
            var toolPackagestoMigrate = GetToolPackageMigrationList(ToolingVersion.Preview1, projectUpgradeContext);
            var migrateToolPackages = new MigrateToolPackages(toolPackagestoMigrate);
            upgradeActions.Add(migrateToolPackages);

            // }

            if (version == ReleaseVersion.RTM)
            {
                // migrates specific nuget packages where their name has completely changed, and also adds new ones that the project may require.
                // this is currently described by a hardcoded list.
                var nugetPackagesToMigrateRTM = ProjectMigrator.GetRtmDependencyPackageMigrationList(ToolingVersion.Preview2, projectUpgradeContext);
                var packageMigrationActionRTM = new MigrateDependencyPackages(nugetPackagesToMigrateRTM);
                upgradeActions.Add(packageMigrationActionRTM);

                // updates remaining microsoft rc2 packages to be rtm version numbers i.e 1.0.0 instead of 1.0.0-rc2-final
                var updateMicrosoftPackageVersionNumbersActionRtm = new UpgradeMicrosoftPackageVersionNumbers(version);
                upgradeActions.Add(updateMicrosoftPackageVersionNumbersActionRtm);


                // migrates old command packages to the new tool nuget packages.
                var toolPackagestoMigrateRtm = GetToolPackageMigrationList(ToolingVersion.Preview2, projectUpgradeContext);
                var migrateToolPackagesRtm = new MigrateToolPackages(toolPackagestoMigrateRtm);
                upgradeActions.Add(migrateToolPackagesRtm);
            }

            return upgradeActions;



        }

        public static List<DependencyPackageMigrationInfo> GetRc2DependencyPackageMigrationList(ToolingVersion targetToolingVersion, IProjectUpgradeContext projectContext)
        {

            var packageMigrationProviders = new List<IDependencyPackageMigrationProvider>();
            packageMigrationProviders.Add(new AspNetDependencyPackageMigrationProvider());
            packageMigrationProviders.Add(new EntityFrameworkDependencyPackageMigrationProvider());
            packageMigrationProviders.Add(new FileProviderDependencyPackageMigrationProvider());
            packageMigrationProviders.Add(new MicrosoftExtensionsDependencyPackageMigrationProvider());
            packageMigrationProviders.Add(new AutofacPackageMigrationProvider());

            var migrations = new List<DependencyPackageMigrationInfo>();
            foreach (var provider in packageMigrationProviders)
            {
                var packageMigrations = provider.GetPackageMigrations(targetToolingVersion, projectContext);
                migrations.AddRange(packageMigrations);
            }

            return migrations;
            // Microsoft.VisualStudio.Web.BrowserLink.Loader?
        }

        public static List<DependencyPackageMigrationInfo> GetRtmDependencyPackageMigrationList(ToolingVersion targetToolingVersion, IProjectUpgradeContext projectContext)
        {

            var packageMigrationProviders = new List<IDependencyPackageMigrationProvider>();
            //    packageMigrationProviders.Add(new AspNetDependencyPackageMigrationProvider());
            packageMigrationProviders.Add(new EntityFrameworkRtmDependencyPackageMigrationProvider());
            packageMigrationProviders.Add(new AspNetRtmDependencyPackageMigrationProvider());
            

            var migrations = new List<DependencyPackageMigrationInfo>();
            foreach (var provider in packageMigrationProviders)
            {
                var packageMigrations = provider.GetPackageMigrations(targetToolingVersion, projectContext);
                migrations.AddRange(packageMigrations);
            }

            return migrations;
            // Microsoft.VisualStudio.Web.BrowserLink.Loader?
        }


        public static List<ToolPackageMigrationInfo> GetToolPackageMigrationList(ToolingVersion targetToolingVersion, IProjectUpgradeContext projectContext)
        {
            var toolMigrationProviders = new List<IToolPackageMigrationProvider>();
            toolMigrationProviders.Add(new CommonToolPackagesMigration());

            var migrations = new List<ToolPackageMigrationInfo>();
            foreach (var provider in toolMigrationProviders)
            {
                var packageMigrations = provider.GetPackageMigrations(targetToolingVersion, projectContext);
                migrations.AddRange(packageMigrations);
            }

            return migrations;
        }



    }
}