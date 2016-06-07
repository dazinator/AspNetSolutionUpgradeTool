using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AspNetUpgrade.Roslyn
{
    public class Rc2Rewriter : CSharpSyntaxRewriter
    {

        public Rc2Rewriter()
        {
            // Populate patterns for using statements we want to replace.
            UsingStatementPatternReplacements = new List<UsingStatementPatternReplacement>();
            var fileProviderReplacement = new UsingStatementPatternReplacement("Microsoft.AspNet.FileProviders.*", "Microsoft.Extensions.FileProviders.*");
            UsingStatementPatternReplacements.Add(fileProviderReplacement);
            var mvcPatternReplacement = new UsingStatementPatternReplacement("Microsoft.AspNet.Mvc.*", "Microsoft.AspNetCore.Mvc.*");
            UsingStatementPatternReplacements.Add(mvcPatternReplacement);
            UsingStatementPatternReplacements.Add(new UsingStatementPatternReplacement("Microsoft.AspNet.Hosting", "Microsoft.AspNetCore.Hosting"));
            UsingStatementPatternReplacements.Add(new UsingStatementPatternReplacement("Microsoft.AspNet.FileProviders", "Microsoft.Extensions.FileProviders"));
            UsingStatementPatternReplacements.Add(new UsingStatementPatternReplacement("Microsoft.AspNet.Mvc", "Microsoft.AspNetCore.Mvc"));
            UsingStatementPatternReplacements.Add(new UsingStatementPatternReplacement("Microsoft.AspNet.StaticFiles", "Microsoft.AspNetCore.StaticFiles"));
            UsingStatementPatternReplacements.Add(new UsingStatementPatternReplacement("Microsoft.AspNet.Hosting", "Microsoft.AspNetCore.Hosting"));
            UsingStatementPatternReplacements.Add(new UsingStatementPatternReplacement("Microsoft.AspNet.Builder", "Microsoft.AspNet.Builder"));

        }

        public List<UsingStatementPatternReplacement> UsingStatementPatternReplacements { get; set; }

        private bool TryMapReplacement(string usingStatementName, out NameSyntax outReplacementUsingName)
        {
            outReplacementUsingName = null;
           
            foreach (var pattern in UsingStatementPatternReplacements)
            {
                if (pattern.TryMapReplacement(usingStatementName, out outReplacementUsingName))
                {
                    return true;
                }
            }

            return false;

        }

        public override SyntaxNode VisitUsingDirective(UsingDirectiveSyntax node)
        {
            var name = node.Name.ToString();

            // replace using statements that we have a mapping for.
            NameSyntax replacementUsing;
            if (TryMapReplacement(name, out replacementUsing))
            {
                var newUsing = node.WithName(replacementUsing);
                return newUsing;
            }

            return base.VisitUsingDirective(node);
        }

      
    }
}