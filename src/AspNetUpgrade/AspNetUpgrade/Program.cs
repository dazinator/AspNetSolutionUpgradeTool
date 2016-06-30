using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AspNetUpgrade.Migrator;
using AspNetUpgrade.Model;
using AspNetUpgrade.UpgradeContext;
using CommandLine;

namespace AspNetUpgrade
{
    class Program
    {
        static int Main(string[] args)
        {
            var result = CommandLine.Parser.Default.ParseArguments<Options.UpgradeSolutionOptions>(args);
            var exitCode = result
              .MapResult((options) =>
              {

                  var solutionDir = new DirectoryInfo(options.SolutionDir);
                  var solutionUpgradeContext = new SolutionUpgradeContext(solutionDir);
                  var solutionMigrator = new SolutionMigrator(solutionUpgradeContext);

                  ToolingVersion toolingVersion = ToolingVersion.Preview1;
                  if (options.ReleaseVersion == ReleaseVersion.RTM)
                  {
                      toolingVersion = ToolingVersion.Preview2;
                  }


                  var solutionMigratorOptions = new SolutionMigrationOptions(toolingVersion);
                  solutionMigrator.Apply(solutionMigratorOptions);

                  // upgrade projects.
                  foreach (var project in solutionUpgradeContext.Projects)
                  {
                      Console.WriteLine("Upgrading " + project.ToString());
                      var projectMigrator = new ProjectMigrator(project);
                      // using default options for each project, but should probably extend command line to set these.
                      var projectUpgradeOptions = new ProjectMigrationOptions(options.ReleaseVersion);
                      projectMigrator.Apply(projectUpgradeOptions);

                      // upgrade csharp files in project if enabled.
                      if (!options.NoCsharpRefactoring)
                      {
                          var codeMigrationoptions = new CsharpCodeMigrationOptions() { RewriteUsings = true };
                          var csharpMigrator = new CsharpCodeMigrator(codeMigrationoptions);

                          foreach (var csharpFileUpgradeContext in project.CsharpFiles)
                          {
                              Console.WriteLine("Refactoring: {0}", csharpFileUpgradeContext.ToString());
                              csharpMigrator.Apply(csharpFileUpgradeContext, null);
                          }
                      }

                  }

                  // save changes to disk.
                  solutionUpgradeContext.SaveChanges();

                  // perform refactorings?

                  //Refactor(solutionUpgradeContext);

                  Console.WriteLine("Deleting project.lock.json files..");
                  var projectLockFiles = solutionDir.GetFileSystemInfos("project.lock.json", SearchOption.AllDirectories);
                  foreach (var projectLock in projectLockFiles)
                  {
                      Console.WriteLine("Deleting lock file  " + projectLock.ToString());
                      projectLock.Delete();
                  }

                  if (options.Verbose)
                  {
                      Console.WriteLine("Upgrade finished.");
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
