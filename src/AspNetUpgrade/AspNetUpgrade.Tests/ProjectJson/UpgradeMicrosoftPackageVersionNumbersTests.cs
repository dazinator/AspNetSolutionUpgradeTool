using ApprovalTests;
using ApprovalTests.Namers;
using ApprovalTests.Reporters;
using AspNetUpgrade.Actions.ProjectJson;
using AspNetUpgrade.Migrator;
using NUnit.Framework;

namespace AspNetUpgrade.Tests.ProjectJson
{
    [UseReporter(typeof(DiffReporter))]
    [TestFixture]
    public class UpgradeMicrosoftPackageVersionNumbersTests
    {

        public UpgradeMicrosoftPackageVersionNumbersTests()
        {

        }

        [TestCase("WebApplicationProject RTM", TestProjectJsonContents.WebApplicationProject, ReleaseVersion.RTM)]
        [TestCase("WebApplicationProject RC2", TestProjectJsonContents.WebApplicationProject, ReleaseVersion.RC2)]
        [TestCase("DnxCore50Project RTM", TestProjectJsonContents.DnxCore50Project, ReleaseVersion.RTM)]
        [TestCase("DnxCore50Project RC2", TestProjectJsonContents.DnxCore50Project, ReleaseVersion.RC2)]
        [TestCase("Dnx451Project RTM", TestProjectJsonContents.Dnx451Project, ReleaseVersion.RTM)]
        [TestCase("Dnx451Project RC2", TestProjectJsonContents.Dnx451Project, ReleaseVersion.RC2)]
        [Test]
        public void Can_Apply(string scenario, string json, ReleaseVersion version)
        {

            using (ApprovalResults.ForScenario(scenario))
            {
                // arrange
                var testFileUpgradeContext = new TestJsonBaseProjectUpgradeContext(json, null, null);
                var sut = new UpgradeMicrosoftPackageVersionNumbers(version);

                // act
                sut.Apply(testFileUpgradeContext);
                testFileUpgradeContext.SaveChanges();

                // assert.
                var modifiedContents = testFileUpgradeContext.ModifiedProjectJsonContents;
                Approvals.VerifyJson(modifiedContents);

            }

        }


    }
}