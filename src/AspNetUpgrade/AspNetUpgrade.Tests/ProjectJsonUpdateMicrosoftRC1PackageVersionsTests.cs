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
    public class ProjectJsonUpdateMicrosoftRC1PackageVersionsTests
    {

        public ProjectJsonUpdateMicrosoftRC1PackageVersionsTests()
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
                var sut = new AspNetUpgrade.Actions.MicrosoftPackagesVersionUpdateAction();

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
                var sut = new AspNetUpgrade.Actions.MicrosoftPackagesVersionUpdateAction();

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