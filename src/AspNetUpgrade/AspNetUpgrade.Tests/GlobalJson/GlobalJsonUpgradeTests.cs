using System.Collections.Generic;
using ApprovalTests;
using ApprovalTests.Namers;
using ApprovalTests.Reporters;
using AspNetUpgrade.Actions;
using AspNetUpgrade.Actions.GlobalJson;
using AspNetUpgrade.Actions.ProjectJson;
using AspNetUpgrade.Model;
using NUnit.Framework;

namespace AspNetUpgrade.Tests.GlobalJson
{
    [UseReporter(typeof(DiffReporter))]
    [TestFixture]
    public class GlobalJsonUpgradeTests
    {

        public GlobalJsonUpgradeTests()
        {

        }

        [TestCase("Rc1Update1", TestGlobalJsonContents.Rc1Update1GlobalJson)]
        [Test]
        public void Can_Apply(string scenario, string globalJson)
        {

            using (ApprovalResults.ForScenario(scenario))
            {
                // arrange

                var upgradeActions = new List<ISolutionUpgradeAction>();
                var testFileUpgradeContext = new TestSolutionUpgradeContext(globalJson);

                // updates the sdk version in global json.
                var runtimeUpgradeAction = new UpdateSdkVersion("1.0.0-preview1-002702");
                upgradeActions.Add(runtimeUpgradeAction);

                // Apply these actions to the global.json file.
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
        
    }
}