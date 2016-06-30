using System.Collections.Generic;
using AspNetUpgrade.Migrator;
using AspNetUpgrade.Model;
using CommandLine;

namespace AspNetUpgrade.Options
{
    class UpgradeSolutionOptions
    {
        [Option('s', "solutionDir", Required = true,
          HelpText = "The solution dir, containing the global.json file, or the folders with your projects in.")]
        public string SolutionDir { get; set; }

        [Option('r', "releaseVersion", Default = Migrator.ReleaseVersion.RTM,
       HelpText = "The release of asp.net to update packages / dependencies to, i.e RC1, RC2 or RTM.")]
        public ReleaseVersion ReleaseVersion { get; set; }

        // Omitting long name, default --verbose
        [Option(
          HelpText = "Prints all messages to standard output.")]
        public bool Verbose { get; set; }



        [Option(HelpText = "Disables csharp code refactoring.", Default = false)]
        public bool NoCsharpRefactoring { get; set; }


    }


}
