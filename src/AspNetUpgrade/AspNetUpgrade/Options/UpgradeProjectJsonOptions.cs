using System.Collections.Generic;
using CommandLine;

namespace AspNetUpgrade.Options
{
    class UpgradeProjectJsonOptions
    {
        [Option('f', "projectJsonFiles", Required = true,
          HelpText = "The project json files to upgrade.")]
        public IEnumerable<string> InputFiles { get; set; }

        // Omitting long name, default --verbose
        [Option(
          HelpText = "Prints all messages to standard output.")]
        public bool Verbose { get; set; }

        [Option('s', "targetVersion", Required = true, Default = "RC2",
          HelpText = "The target version to migrate projects too.")]
        public string TargetVersion { get; set; }
       
    }


}
