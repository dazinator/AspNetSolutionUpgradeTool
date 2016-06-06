using System;
using System.Collections.Generic;
using System.Linq;
using ApprovalTests;
using ApprovalTests.Namers;
using ApprovalTests.Reporters;
using AspNetUpgrade.Actions;
using AspNetUpgrade.Tests.XProj;
using AspNetUpgrade.Upgrader;
using NUnit.Framework;

namespace AspNetUpgrade.Tests.ProjectJson
{
    [UseReporter(typeof(DiffReporter))]
    [TestFixture]
    public class ProjectUpgradeApprovalTests
    {

        public ProjectUpgradeApprovalTests()
        {

        }


        //[TestCase("LibraryProject", TestProjectJsonContents.LibraryProjectRc1, TestXProjContents.WebApplication)]
        //[TestCase("ConsoleProject", TestProjectJsonContents.ConsoleProjectRc1, TestXProjContents.WebApplication)]
        [TestCase("WebApplicationProject", TestProjectJsonContents.WebApplicationProject, TestXProjContents.WebApplication)]
        [Test]
        public void Can_Apply(string scenario, string json, string xproj)
        {

            using (ApprovalResults.ForScenario(scenario + "_project_json"))
            {
                // arrange
                var testXProj = VsProjectHelper.LoadTestProject(xproj);
                var testFileUpgradeContext = new TestJsonProjectUpgradeContext(json, testXProj);

                var migrator = new ProjectMigrator(testFileUpgradeContext);
                var options = new MigrationOptions();

                options.UpgradeProjectFilesToPreview1 = true; // project.json files will be updated to the preview 1 schema.
                options.UpgradePackagesToRC2 = true; // rc1 packages will be migrated to rc2 packages, including commands (migrated to tools).
                options.AddNetStandardTargetToLibraries = true; // libraries will have the netStandard TFM added (and dependency).
                options.AddNetCoreTargetToApplications = true; // applications will have the netCore app TFM added (and dependency)

                // migrate 
                migrator.Apply(options);

                // save the changes.
                testFileUpgradeContext.SaveChanges();

                // assert.
                var modifiedContents = testFileUpgradeContext.ModifiedJsonContents;
                Approvals.VerifyJson(modifiedContents);


                using (ApprovalResults.ForScenario(scenario + "_xproj"))
                {
                    var projContents = VsProjectHelper.ToString(testFileUpgradeContext.VsProjectFile);
                    Approvals.VerifyXml(projContents);
                }

            }


        }

       
        //[TestCase("LibraryProject", TestProjectJsonContents.LibraryProjectRc1)]
        //[TestCase("ConsoleProject", TestProjectJsonContents.ConsoleProjectRc1)]
        [TestCase("WebApplicationProject", TestProjectJsonContents.WebApplicationProject, TestXProjContents.WebApplication)]
        [Test]
        public void Can_Rollback_If_Error(string scenario, string json, string xproj)
        {

            using (ApprovalResults.ForScenario(scenario + "_project_json"))
            {
                // arrange
                var testXProj = VsProjectHelper.LoadTestProject(xproj);
                var testFileUpgradeContext = new TestJsonProjectUpgradeContext(json, testXProj);
                var migrator = new ProjectMigrator(testFileUpgradeContext);
                var options = new MigrationOptions();

                options.UpgradeProjectFilesToPreview1 = true; // project.json files will be updated to the preview 1 schema.
                options.UpgradePackagesToRC2 = true; // rc1 packages will be migrated to rc2 packages, including commands (migrated to tools).
                options.AddNetStandardTargetToLibraries = true; // libraries will have the netStandard TFM added (and dependency).
                options.AddNetCoreTargetToApplications = true; // applications will have the netCore app TFM added (and dependency)
                
                // add an upgrade that throws an exception..
                var additionalUpgrades = new List<IJsonUpgradeAction>();
                additionalUpgrades.Add(new ExceptionuringUpgradeAction());

             
                try
                {
                    // migrate 
                    migrator.Apply(options, additionalUpgrades);
                    testFileUpgradeContext.SaveChanges();
                }
                catch (Exception e)
                {
                    // throw;
                }

                // assert.
                var modifiedContents = testFileUpgradeContext.JsonObject.ToString();
                Approvals.VerifyJson(modifiedContents);

                using (ApprovalResults.ForScenario(scenario + "_xproj"))
                {
                    var projContents = VsProjectHelper.ToString(testFileUpgradeContext.VsProjectFile);
                    Approvals.VerifyXml(projContents);
                }

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