using ApprovalTests;
using ApprovalTests.Namers;
using ApprovalTests.Reporters;
using AspNetUpgrade.Actions.ProjectJson;
using AspNetUpgrade.Model;
using AspNetUpgrade.Upgrader;
using NUnit.Framework;

namespace AspNetUpgrade.Tests.ProjectJson
{
    [UseReporter(typeof(DiffReporter))]
    [TestFixture]
    public class MigrateDependencyPackagesTests
    {

        public MigrateDependencyPackagesTests()
        {

        }

        [TestCase("WebApplicationProject", TestProjectJsonContents.WebApplicationProject)]
        [Test]
        public void Can_Apply(string scenario, string json)
        {

            using (ApprovalResults.ForScenario(scenario))
            {
                // arrange
                var testFileUpgradeContext = new TestJsonProjectUpgradeContext(json);
                // get target nuget packages for RC2, Preview1 tooling.
                var targetNuGetPackages =
                    ProjectJsonMigrator.GetRc2DependencyPackageMigrationList(ToolingVersion.Preview1,
                        testFileUpgradeContext);

                //  PackageMigrationHelper.GetRc2DependencyPackageMigrationList(ToolingVersion.Preview1, testFileUpgradeContext);

                var sut = new MigrateDependencyPackages(targetNuGetPackages);

                // act
                sut.Apply(testFileUpgradeContext);
                testFileUpgradeContext.SaveChanges();

                // assert.
                var modifiedContents = testFileUpgradeContext.ModifiedJsonContents;
                Approvals.VerifyJson(modifiedContents);

            }


        }

     



    }
}