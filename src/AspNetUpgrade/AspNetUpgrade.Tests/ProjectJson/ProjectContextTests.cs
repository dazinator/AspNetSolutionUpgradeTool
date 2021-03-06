using ApprovalTests;
using ApprovalTests.Namers;
using ApprovalTests.Reporters;
using AspNetUpgrade.Actions.ProjectJson;
using AspNetUpgrade.Model;
using NUnit.Framework;

namespace AspNetUpgrade.Tests.ProjectJson
{
    [UseReporter(typeof(DiffReporter))]
    [TestFixture]
    public class ProjectUpgradeContextTests
    {

        public ProjectUpgradeContextTests()
        {

        }


        [TestCase(ProjectType.Library, TestProjectJsonContents.LibraryProjectRc1)]
        [TestCase(ProjectType.Application, TestProjectJsonContents.ConsoleProjectRc1)]
        [TestCase(ProjectType.Application, TestProjectJsonContents.WebApplicationProject)]
        [Test]
        public void Can_Detect_Application_Versus_Library_Projects(ProjectType projectType, string json)
        {
            // arrange
            var testFileUpgradeContext = new TestJsonBaseProjectUpgradeContext(json, null, null);
            Assert.AreEqual(testFileUpgradeContext.ToProjectJsonWrapper().GetProjectType(), projectType);
        }





    }
}