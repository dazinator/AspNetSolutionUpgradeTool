using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApprovalTests;
using ApprovalTests.Namers;
using ApprovalTests.Reporters;
using AspNetUpgrade.Actions;
using NUnit.Framework;

namespace AspNetUpgrade.Tests
{

    [UseReporter(typeof(DiffReporter), typeof(WinMergeReporter))]
    [TestFixture]
    public class ProjectJsonFrameworksUpgradeTests
    {

        public ProjectJsonFrameworksUpgradeTests()
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
                var sut = new AspNetUpgrade.Actions.UpgradeFrameworksJson();

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
                var sut = new AspNetUpgrade.Actions.UpgradeFrameworksJson();

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