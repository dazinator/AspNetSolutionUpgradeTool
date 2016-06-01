using ApprovalTests;
using ApprovalTests.Namers;
using ApprovalTests.Reporters;
using AspNetUpgrade.Actions;
using NUnit.Framework;

namespace AspNetUpgrade.Tests
{
    [UseReporter(typeof(DiffReporter), typeof(WinMergeReporter))]
    [TestFixture]
    public class ProjectJsonApplyExplicitPackageRenamesUpgradeTests
    {

        public ProjectJsonApplyExplicitPackageRenamesUpgradeTests()
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
                var testFileUpgradeContext = new TestFileUpgradeContext(json);
                // get target nuget packages for RC2, Preview1 tooling.
                var targetNuGetPackages =
                    MicrosoftPackageRenamingConventionHelper.GetRc2NuGetPackagesList(MicrosoftPackageRenamingConventionHelper.ToolingVersion.Preview1);

                var sut = new AspNetUpgrade.Actions.MigrateSpecifiedPackagesAction(targetNuGetPackages);

                // act
                sut.Apply(testFileUpgradeContext);
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
                var testFileUpgradeContext = new TestFileUpgradeContext(json);
                // get target nuget packages for RC2, Preview1 tooling.
                var targetNuGetPackages =
                    MicrosoftPackageRenamingConventionHelper.GetRc2NuGetPackagesList(MicrosoftPackageRenamingConventionHelper.ToolingVersion.Preview1);

                var sut = new AspNetUpgrade.Actions.MigrateSpecifiedPackagesAction(targetNuGetPackages);

                // act
                sut.Apply(testFileUpgradeContext);
                sut.Undo(testFileUpgradeContext);

                testFileUpgradeContext.SaveChanges();

                // assert.
                var modifiedContents = testFileUpgradeContext.ModifiedJsonContents;
                Approvals.VerifyJson(modifiedContents);

            }

        }



    }
}