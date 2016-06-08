using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AspNetUpgrade.Actions.Csharp
{
    public class UsingStatementPatternReplacement
    {

        public UsingStatementPatternReplacement(string matchPattern, string replacePattern)
        {
            MatchPattern = matchPattern;
            ReplacePattern = replacePattern;
            var pattern = WildcardToRegex(MatchPattern);
            MatchRegex = new Regex(pattern, RegexOptions.IgnoreCase);
            IsPattern = matchPattern.Contains("*");
        }

        public UsingStatementPatternReplacement() : base()
        {
        }

        public bool IsPattern { get; set; }

        public Regex MatchRegex { get; set; }

        public string MatchPattern { get; set; }

        public string ReplacePattern { get; set; }

        public bool TryMapReplacement(string usingStatementName, out NameSyntax outReplacementUsingName)
        {
            outReplacementUsingName = null;

            if (!IsPattern)
            {
                if (usingStatementName == MatchPattern)
                {
                    outReplacementUsingName = BuildName(ReplacePattern);
                    return true;
                }
            }

            // if the source name, matches everything but the star.
            // split sourcenames into segments, and treat wildcard segment as match.

            // split using statement into segments.
            // see if the match pattern segments match these segments.
            if (IsPatternMatch(usingStatementName))
            {
                // need to calculate replacement.
                string replacementUsingName = Replace(usingStatementName);
                outReplacementUsingName = BuildName(replacementUsingName);
                return true;

            }

            // perhaps we have a pattern match.
            return false;

        }

        private string Replace(string usingStatementName)
        {

            if (ReplacePattern.Contains("*"))
            {
                // each segment that has a * indicates that it should stay the same as the matched segment. If it doesn't have a * then it should be replaced.
                // If it's the last segment and it's a * then just leave;
                var usingNameSplit = usingStatementName.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                var replacementString = ReplaceWithPattern(usingNameSplit, ReplacePattern);
                return replacementString;
            }
            else
            {
                // constant.
                return ReplacePattern;
            }


        }

        private string ReplaceWithPattern(string[] usingNameSplit, string replacePattern)
        {

            // examples of matches
            var replacePatternSplit = replacePattern.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

            // each segment that has a * indicates that it should stay the same as the matched segment. If it doesn't have a * then it should be replaced.
            // If it's the last segment and it's a * then just leave;

            var newName = new List<string>();
            var lastReplaceSegment = replacePatternSplit.Last();

            int currentSegment = 0;
            foreach (var segment in usingNameSplit)
            {
                if (replacePatternSplit.Length >= currentSegment + 1)
                {
                    var replaceSegment = replacePatternSplit[currentSegment];
                    if (replaceSegment != "*")
                    {
                        newName.Add(replaceSegment);
                    }
                    else
                    {
                        newName.Add(segment);
                    }
                }
                else
                {
                    // we don't have a replacement segment in the replace pattern for this segment, 
                    // as long as the last pattenr is a * then leave it as is, else remove  it.
                    if (lastReplaceSegment == "*")
                    {
                        newName.Add(segment);
                    }
                    else
                    {
                        // don't include this as no longer matches the pattern.
                    }
                }

                currentSegment = currentSegment + 1;

            }

            return string.Join(".", newName);

        }

        private bool IsPatternMatch(string usingStatementName)
        {
            if (MatchRegex.IsMatch(usingStatementName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string WildcardToRegex(string pattern)
        {
            return "^" + Regex.Escape(pattern)
                .Replace(@"\*", ".*")
                .Replace(@"\?", ".")
                   + "$";
        }

        private NameSyntax BuildName(string replacement)
        {
            var parts = replacement.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            var first = parts.First();
            NameSyntax name = SyntaxFactory.IdentifierName(first);
            foreach (var part in parts.Skip(1))
            {
                name = SyntaxFactory.QualifiedName(name, SyntaxFactory.IdentifierName(part));
            }
            return name;
        }
      
    }
}