using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApprovalTests;
using ApprovalTests.Reporters;
using AspNetUpgrade.Actions;
using NUnit.Framework;

namespace AspNetUpgrade.Tests
{

    [UseReporter(typeof(DiffReporter))]
    [TestFixture]
    public class ProjectJsonFrameworksUpgradeTests
    {

        public ProjectJsonFrameworksUpgradeTests()
        {

        }

        [TestCase(TestProjectJsonContents.Dnx451Project)]
        [Test]
        public void Can_Apply(string json)
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

        [TestCase(TestProjectJsonContents.Dnx451Project)]
        [Test]
        public void Can_Undo(string json)
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