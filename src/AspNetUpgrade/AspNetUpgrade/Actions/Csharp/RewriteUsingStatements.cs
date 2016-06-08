using AspNetUpgrade.UpgradeContext;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AspNetUpgrade.Actions.Csharp
{
    /// <summary>
    /// Ensures any applications target .NET Core, by adding .NET Core to the frameworks.
    /// </summary>
    public class RewriteUsingStatements : ICsharpCodeUpgradeAction
    {


        private Rc2Rewriter _rewriter;

        public RewriteUsingStatements(Rc2Rewriter rewriter)
        {
            _rewriter = rewriter;
        }


        public void Apply(ICsharpFileUpgradeContext fileUpgradeContext)
        {

            var root = (CompilationUnitSyntax)fileUpgradeContext.Source.GetRoot();
            // fileUpgradeContext.Source = _rewriter.Visit(root).SyntaxTree;
            SyntaxNode newSource = _rewriter.Visit(root);
            fileUpgradeContext.Source = newSource.SyntaxTree;

            //if (newSource != root)
            //{
            //    File.WriteAllText(sourceTree.FilePath, newSource.ToFullString());
            //}

            //var collector = new UsingCollector();
            // collector.Visit(root);

        }



    }
}