using ApprovalTests;
using ApprovalTests.Namers;
using ApprovalTests.Reporters;
using AspNetUpgrade.Actions.ProjectJson;
using AspNetUpgrade.Model;
using NUnit.Framework;

namespace AspNetUpgrade.Tests.ProjectJson
{
    [UseReporter(typeof(DiffReporter))]
    [TestFixture]
    public class MigrateSpecifiedPackagesTests
    {

        public MigrateSpecifiedPackagesTests()
        {

        }

        [TestCase("WebApplicationProject", TestProjectJsonContents.WebApplicationProject)]
        [Test]
        public void Can_Apply(string scenario, string json)
        {

            using (ApprovalResults.ForScenario(scenario))
            {
                // arrange
                var testFileUpgradeContext = new TestFileUpgradeContext(json);
                // get target nuget packages for RC2, Preview1 tooling.
                var targetNuGetPackages =
                    PackageMigrationHelper.GetRc2PackageMigrationList(PackageMigrationHelper.ToolingVersion.Preview1);

                var sut = new MigrateSpecifiedPackages(targetNuGetPackages);

                // act
                sut.Apply(testFileUpgradeContext);
                testFileUpgradeContext.SaveChanges();

                // assert.
                var modifiedContents = testFileUpgradeContext.ModifiedJsonContents;
                Approvals.VerifyJson(modifiedContents);

            }


        }

        [TestCase("WebApplicationProject", TestProjectJsonContents.WebApplicationProject)]
        [Test]
        public void Can_Undo(string scenario, string json)
        {

            using (ApprovalResults.ForScenario(scenario))
            {
                // arrange
                var testFileUpgradeContext = new TestFileUpgradeContext(json);
                // get target nuget packages for RC2, Preview1 tooling.
                var targetNuGetPackages =
                    PackageMigrationHelper.GetRc2PackageMigrationList(PackageMigrationHelper.ToolingVersion.Preview1);

                var sut = new MigrateSpecifiedPackages(targetNuGetPackages);

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