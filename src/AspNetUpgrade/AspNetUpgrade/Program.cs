using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommandLine;

namespace AspNetUpgrade
{
    class Program
    {
        static int Main(string[] args)
        {
            var result = CommandLine.Parser.Default.ParseArguments<Options.UpgradeProjectJsonOptions>(args);
            var exitCode = result
              .MapResult((options) =>
              {
                  if (options.Verbose)
                  {
                      Console.WriteLine("Filenames: {0}", string.Join(",", options.InputFiles.ToArray()));
                  }
                  return 0;
              },
              (errors) =>
              {
                  //LogHelper.Log(errors);
                  return 1;
              });

            return exitCode;
        }
    }
}
