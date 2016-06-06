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

        //[Option('s', "targetVersion", Required = true, Default = "RC2",
        //  HelpText = "The target version to migrate projects too.")]
        //public bool TargetVersion { get; set; }
       
    }


}
