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
            var runtimeUpgradeAction = new AddRuntimeDependency();
            upgradeActions.Add(runtimeUpgradeAction);

            // upgrades the compilation options section.
            var compilationOptionsUpgradeAction = new UpgradeCompilationOptionsJson();
            upgradeActions.Add(compilationOptionsUpgradeAction);

            // upgrades the frameworks section.
            var frameworksUpgradeAction = new UpgradeFrameworksJson();
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

            return upgradeActions;

        }
    }
}