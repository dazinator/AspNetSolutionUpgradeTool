using System;
using System.Collections.Generic;
using ApprovalTests;
using ApprovalTests.Namers;
using ApprovalTests.Reporters;
using AspNetUpgrade.Actions;
using AspNetUpgrade.Actions.GlobalJson;
using AspNetUpgrade.Actions.Xproj;
using NUnit.Framework;

namespace AspNetUpgrade.Tests.XProj
{
    [UseReporter(typeof(DiffReporter))]
    [TestFixture]
    public class XProjJsonUpgradeTests
    {

        public XProjJsonUpgradeTests()
        {

        }

        //[TestCase("Library", TestProjectJsonContents.LibraryProjectRc1, TestXProjContents.WebApplication)]
        [TestCase("WebApplication", TestProjectJsonContents.WebApplicationProject, TestXProjContents.WebApplication)]
        [Test]
        public void Can_Apply(string scenario, string json, string xproj)
        {

            using (ApprovalResults.ForScenario(scenario))
            {
                // arrange

                var upgradeActions = new List<IJsonUpgradeAction>();
                var testXProj = VsProjectFactory.LoadTestProject(xproj);

                var testFileUpgradeContext = new TestJsonProjectUpgradeContext(json, testXProj);

                // updates the sdk version in global json.
                var runtimeUpgradeAction = new MigrateProjectImportsFromDnxToDotNet();
                upgradeActions.Add(runtimeUpgradeAction);

                // Apply these actions to the project.json file.
                foreach (var upgradeAction in upgradeActions)
                {
                    upgradeAction.Apply(testFileUpgradeContext);
                }

                // save the changes.
                testFileUpgradeContext.SaveChanges();

                var projContents = VsProjectHelper.ToString(testFileUpgradeContext.VsProjectFile);
                Approvals.VerifyXml(projContents);


                // assert.
                //var modifiedContents = testFileUpgradeContext.ModifiedJsonContents;
                //Approvals.VerifyJson(modifiedContents);

                // Approvals.VerifyAll();

            }

        }



    }
}