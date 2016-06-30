using System;
using System.IO;
using System.Reflection;
using ApprovalTests;
using ApprovalTests.Namers;
using ApprovalTests.Reporters;
using AspNetUpgrade.Actions.Csharp;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;

namespace AspNetUpgrade.Tests.Csharp
{

    [UseReporter(typeof(DiffReporter))]
    [TestFixture]
    public class RewriteUsingsTests
    {

        [TestCase("CsharpFile", "TestCsharpFiles\\TestCsharpFile.cs")]
        [Test]
        public void Can_Replace_Old_Using_Statements(string scenario, string filePath)
        {

            var assemblyDir = AssemblyDirectory;
            using (ApprovalResults.ForScenario(scenario))
            {
                var testFilePath = System.IO.Path.Combine(assemblyDir, filePath);
                //  var fileInfo = new FileInfo(testFilePath);
                var fileText = System.IO.File.ReadAllText(testFilePath);
                SyntaxTree tree = CSharpSyntaxTree.ParseText(fileText);


                var newSource = FixUp(tree);
                //if (newSource != sourceTree.GetRoot())
                //{
                //    File.WriteAllText(sourceTree.FilePath, newSource.ToFullString());
                //}

                // assert.
                Approvals.VerifyJson(newSource.ToString());

            }


        }



        private SyntaxNode FixUp(SyntaxTree tree)
        {
            var root = (CompilationUnitSyntax)tree.GetRoot();
            var rewriter = new Rc2Rewriter();
            SyntaxNode newSource = rewriter.Visit(root);
            return newSource;
        }

        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }


    }
}
