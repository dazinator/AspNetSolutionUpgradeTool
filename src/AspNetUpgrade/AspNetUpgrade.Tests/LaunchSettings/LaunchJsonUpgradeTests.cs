using System.Collections.Generic;
using ApprovalTests;
using ApprovalTests.Namers;
using ApprovalTests.Reporters;
using AspNetUpgrade.Actions;
using AspNetUpgrade.Actions.GlobalJson;
using AspNetUpgrade.Actions.LaunchSettings;
using AspNetUpgrade.Migrator;
using AspNetUpgrade.Model;
using NUnit.Framework;

namespace AspNetUpgrade.Tests.LaunchSettings
{
    [UseReporter(typeof(DiffReporter))]
    [TestFixture]
    public class LaunchJsonUpgradeTests
    {

        public LaunchJsonUpgradeTests()
        {

        }

        [TestCase("WebApplicationProject", TestProjectJsonContents.WebApplicationProject, TestLaunchSettingsContents.Rc1LaunchSettings)]
        [Test]
        public void Can_Apply(string scenario, string json, string launchSettings)
        {

            using (ApprovalResults.ForScenario(scenario))
            {
                // arrange
                var testFileUpgradeContext = new TestJsonBaseProjectUpgradeContext(json, null, launchSettings);
             

                var sut = new UpdateLaunchSettings();

                // act
                sut.Apply(testFileUpgradeContext);
                testFileUpgradeContext.SaveChanges();

                // assert.
                var modifiedContents = testFileUpgradeContext.ModifiedLaunchSettingsJsonContents;
                Approvals.VerifyJson(modifiedContents);

            }


        }

    }
}