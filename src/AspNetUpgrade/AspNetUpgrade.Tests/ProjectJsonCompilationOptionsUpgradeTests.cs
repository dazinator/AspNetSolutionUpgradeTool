using ApprovalTests;
using ApprovalTests.Namers;
using ApprovalTests.Reporters;
using NUnit.Framework;

namespace AspNetUpgrade.Tests
{
    [UseReporter(typeof(DiffReporter), typeof(WinMergeReporter))]
    [TestFixture]
    public class ProjectJsonCompilationOptionsUpgradeTests
    {

        public ProjectJsonCompilationOptionsUpgradeTests()
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
                var sut = new AspNetUpgrade.Actions.UpgradeCompilationoptionsJson();

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
                var sut = new AspNetUpgrade.Actions.UpgradeCompilationoptionsJson();

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