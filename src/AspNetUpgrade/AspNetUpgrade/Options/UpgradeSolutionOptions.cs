using System.Collections.Generic;
using CommandLine;

namespace AspNetUpgrade.Options
{
    class UpgradeSolutionOptions
    {
        [Option('s', "solutionDir", Required = true,
          HelpText = "The solution dir, containing the global.json file, or the folders with your projects in.")]
        public string SolutionDir { get; set; }

        // Omitting long name, default --verbose
        [Option(
          HelpText = "Prints all messages to standard output.")]
        public bool Verbose { get; set; }


        [Option(HelpText = "Disables csharp code refactoring.", Default = false)]
        public bool NoCsharpRefactoring { get; set; }


    }


}
