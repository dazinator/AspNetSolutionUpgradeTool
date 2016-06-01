using System.Collections.Generic;
using ApprovalTests;
using ApprovalTests.Namers;
using ApprovalTests.Reporters;
using AspNetUpgrade.Actions;
using NUnit.Framework;

namespace AspNetUpgrade.Tests
{
    [UseReporter(typeof(DiffReporter), typeof(WinMergeReporter))]
    [TestFixture]
    public class CompleteProjectJsonUpgradeTests
    {

        public CompleteProjectJsonUpgradeTests()
        {

        }

        [TestCase("WebApplicationProject", TestProjectJsonContents.WebApplicationProject)]
        [TestCase("DnxCore50Project", TestProjectJsonContents.DnxCore50Project)]
        [TestCase("Dnx451Project", TestProjectJsonContents.Dnx451Project)]
        [Test]
        public void Can_Apply(string scenario, string json)
        {

            using (ApprovalResults.ForScenario(scenario))
            {
                // arrange

                var upgradeActions = new List<IAction>();

                var testFileUpgradeContext = new TestFileUpgradeContext(json);

                // adds the runtime to the dependencies
                var runtimeUpgradeAction = new AspNetUpgrade.Actions.AddRuntimeDependencyJson();
                upgradeActions.Add(runtimeUpgradeAction);

                // upgrades the compilation options section.
                var compilationOptionsUpgradeAction = new AspNetUpgrade.Actions.UpgradeCompilationoptionsJson();
                upgradeActions.Add(compilationOptionsUpgradeAction);

                // upgrades the frameworks section.
                var frameworksUpgradeAction = new AspNetUpgrade.Actions.UpgradeFrameworksJson();
                upgradeActions.Add(frameworksUpgradeAction);

                // migrates specific nuget packages where their name has completely changed, this is currently described by a hardcoded list.
                var nugetPackagesToMigrate = MicrosoftPackageRenamingConventionHelper.GetRc2NuGetPackagesList(MicrosoftPackageRenamingConventionHelper.ToolingVersion.Preview1);
                var packageMigrationAction = new AspNetUpgrade.Actions.MigrateSpecifiedPackagesAction(nugetPackagesToMigrate);
                upgradeActions.Add(packageMigrationAction);

                // renames microsoft.aspnet packages to be microsoft.aspnetcore.
                var renameAspNetPackagesAction = new AspNetUpgrade.Actions.RenameAspNetPackagesAction();
                upgradeActions.Add(renameAspNetPackagesAction);

                // updates microsoft. packages to be rc2 version numbers.
                var updateMicrosoftPackageVersionNumbersAction = new AspNetUpgrade.Actions.MicrosoftPackagesVersionUpdateAction();
                upgradeActions.Add(updateMicrosoftPackageVersionNumbersAction);

                // Apply these actions to the project.json file.
                foreach (var upgradeAction in upgradeActions)
                {
                    upgradeAction.Apply(testFileUpgradeContext);
                }

                // save the changes.
                testFileUpgradeContext.SaveChanges();

                // assert.
                var modifiedContents = testFileUpgradeContext.ModifiedJsonContents;
                Approvals.VerifyJson(modifiedContents);

            }


        }

        [TestCase("WebApplicationProject", TestProjectJsonContents.WebApplicationProject)]
        [TestCase("DnxCore50Project", TestProjectJsonContents.DnxCore50Project)]
        [TestCase("Dnx451Project", TestProjectJsonContents.Dnx451Project)]
        [Test]
        public void Can_Undo(string scenario, string json)
        {

            using (ApprovalResults.ForScenario(scenario))
            {
                // arrange

                var upgradeActions = new List<IAction>();

                var testFileUpgradeContext = new TestFileUpgradeContext(json);


                var runtimeUpgradeAction = new AspNetUpgrade.Actions.AddRuntimeDependencyJson();
                upgradeActions.Add(runtimeUpgradeAction);

                var compilationOptionsUpgradeAction = new AspNetUpgrade.Actions.UpgradeCompilationoptionsJson();
                upgradeActions.Add(compilationOptionsUpgradeAction);

                var frameworksUpgradeAction = new AspNetUpgrade.Actions.UpgradeFrameworksJson();
                upgradeActions.Add(frameworksUpgradeAction);


                var nugetPackagesToMigrate = MicrosoftPackageRenamingConventionHelper.GetRc2NuGetPackagesList(MicrosoftPackageRenamingConventionHelper.ToolingVersion.Preview1);
                var packageMigrationAction = new AspNetUpgrade.Actions.MigrateSpecifiedPackagesAction(nugetPackagesToMigrate);
                upgradeActions.Add(packageMigrationAction);


                foreach (var upgradeAction in upgradeActions)
                {
                    upgradeAction.Apply(testFileUpgradeContext);
                }

                upgradeActions.Reverse();

                foreach (var upgradeAction in upgradeActions)
                {
                    upgradeAction.Undo(testFileUpgradeContext);
                }

                testFileUpgradeContext.SaveChanges();

                // assert.
                var modifiedContents = testFileUpgradeContext.ModifiedJsonContents;
                Approvals.VerifyJson(modifiedContents);

            }

        }



    }
}