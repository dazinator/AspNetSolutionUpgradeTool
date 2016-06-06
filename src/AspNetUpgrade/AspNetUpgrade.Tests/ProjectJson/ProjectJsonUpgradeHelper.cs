using System.Collections.Generic;
using AspNetUpgrade.Actions;
using AspNetUpgrade.Actions.ProjectJson;
using AspNetUpgrade.Model;
using AspNetUpgrade.Upgrader;

namespace AspNetUpgrade.Tests.ProjectJson
{
    public static class ProjectJsonUpgradeHelper
    {
        public static IList<IJsonUpgradeAction> GetProjectJsonUpgrades(IJsonProjectUpgradeContext projectUpgradeContext)
        {
            var upgradeActions = new List<IJsonUpgradeAction>();

            // adds the runtime to the dependencies

            // Makes applications target .netcoreapp.
            var addNetCoreApp = new AddNetCoreFrameworkToApplications("1.0.0-rc2-3002702");
            upgradeActions.Add(addNetCoreApp);

            // Adds the netStandard framework, with specified version of NETStandard.Library dependency, to any library project.json's.
            var addNetStandard = new AddNetStandardFrameworkToLibrariesJson("netstandard1.5", "1.5.0-rc2-24027");
            upgradeActions.Add(addNetStandard);

            // upgrades the compilation options section.
            var compilationOptionsUpgradeAction = new UpgradeCompilationOptionsJson();
            upgradeActions.Add(compilationOptionsUpgradeAction);

            // moves things to packOptions.
            var upgradePackOptions = new UpgradePackOptions();
            upgradeActions.Add(upgradePackOptions);

            // moves content to the new packOptions and buildOptions / copyToOutput
            var moveContent = new MoveContentToNewOptions();
            upgradeActions.Add(moveContent);

            // upgrades the frameworks section.
            var frameworksUpgradeAction = new MigrateDnxFrameworksToNetFrameworksJson();
            upgradeActions.Add(frameworksUpgradeAction);

            // migrates specific nuget packages where their name has completely changed, this is currently described by a hardcoded list.
            var nugetPackagesToMigrate = PackageMigrationHelper.GetRc2DependencyPackageMigrationList(ToolingVersion.Preview1, projectUpgradeContext);
            var packageMigrationAction = new MigrateDependencyPackages(nugetPackagesToMigrate);
            upgradeActions.Add(packageMigrationAction);

            // renames microsoft.aspnet packages to be microsoft.aspnetcore.
            var renameAspNetPackagesAction = new RenameAspNetPackagesToAspNetCore();
            upgradeActions.Add(renameAspNetPackagesAction);

            // updates microsoft. packages to be rc2 version numbers.
            var updateMicrosoftPackageVersionNumbersAction = new UpgradeMicrosoftPackageVersionNumbers();
            upgradeActions.Add(updateMicrosoftPackageVersionNumbersAction);

            // renames "commands" to "tools" and clears commands..
            var renameCommandstoToolsAndClear = new RenameCommandsToTools();
            upgradeActions.Add(renameCommandstoToolsAndClear);

            // apply tool migrations..
            var toolPackagestoMigrate = PackageMigrationHelper.GetRc2ToolPackageMigrationList(ToolingVersion.Preview1, projectUpgradeContext);
            var migrateToolPackages = new MigrateToolPackages(toolPackagestoMigrate);
            upgradeActions.Add(migrateToolPackages);

            var upgradePublishOptions = new UpgradePublishOptions();
            upgradeActions.Add(upgradePublishOptions);

            return upgradeActions;

        }
    }
}