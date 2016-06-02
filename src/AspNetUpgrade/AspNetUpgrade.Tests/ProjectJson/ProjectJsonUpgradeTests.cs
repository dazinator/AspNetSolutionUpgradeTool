using System;
using System.Linq;
using ApprovalTests;
using ApprovalTests.Namers;
using ApprovalTests.Reporters;
using AspNetUpgrade.Actions;
using AspNetUpgrade.Upgrader;
using NUnit.Framework;

namespace AspNetUpgrade.Tests.ProjectJson
{
    [UseReporter(typeof(DiffReporter))]
    [TestFixture]
    public class ProjectJsonUpgradeTests
    {

        public ProjectJsonUpgradeTests()
        {

        }

        [TestCase("WebApplicationProject", TestProjectJsonContents.WebApplicationProject)]
        [TestCase("LibraryProject", TestProjectJsonContents.LibraryProjectRc1)]
        [TestCase("ConsoleProject", TestProjectJsonContents.ConsoleProjectRc1)]
        [Test]
        public void Can_Apply(string scenario, string json)
        {

            using (ApprovalResults.ForScenario(scenario))
            {
                // arrange
                var testFileUpgradeContext = new TestJsonProjectUpgradeContext(json);
                var upgrades = ProjectJsonUpgradeHelper.GetProjectJsonUpgrades(testFileUpgradeContext);

                var migrator = new ProjectMigrator(testFileUpgradeContext);

                //Act
                migrator.Apply(upgrades);

                // save the changes.
                testFileUpgradeContext.SaveChanges();

                // assert.
                var modifiedContents = testFileUpgradeContext.ModifiedJsonContents;
                Approvals.VerifyJson(modifiedContents);

            }


        }

        [TestCase("WebApplicationProject", TestProjectJsonContents.WebApplicationProject)]
        [TestCase("LibraryProject", TestProjectJsonContents.LibraryProjectRc1)]
        [TestCase("ConsoleProject", TestProjectJsonContents.ConsoleProjectRc1)]
        [Test]
        public void Can_Rollback_If_Error(string scenario, string json)
        {

            using (ApprovalResults.ForScenario(scenario))
            {
                // arrange

                var testFileUpgradeContext = new TestJsonProjectUpgradeContext(json);
                var upgrades = ProjectJsonUpgradeHelper.GetProjectJsonUpgrades(testFileUpgradeContext);
                // add an upgrade that throws an exception..
                upgrades.Add(new ExceptionuringUpgradeAction());

                var migrator = new ProjectMigrator(testFileUpgradeContext);
                try
                {
                    migrator.Apply(upgrades);
                    testFileUpgradeContext.SaveChanges();
                }
                catch (Exception e)
                {
                    // throw;
                }

                // assert.
                var modifiedContents = testFileUpgradeContext.JsonObject.ToString();
                Approvals.VerifyJson(modifiedContents);

            }

        }

        public class ExceptionuringUpgradeAction : IJsonUpgradeAction
        {
            public void Apply(IJsonProjectUpgradeContext fileUpgradeContext)
            {
                throw new System.NotImplementedException();
            }
          
        }





    }
}