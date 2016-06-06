# AspNetRC1toRC2UpgradeTool
A utility that can be used to upgrade ASP.NET 5 RC1 based projects / solutions.

This tool provides options to:

1. Upgrade the `project.json`, `.xproj`, and `global.json` files to the new `Preview1` format which is based on `dotnet cli` and not `dnx`. This includes also renaming the old `dnx4YZ` TFM's in project.json to be the equivalent `net4YZ` TFM.
2. Upgrade RC1 packages (dependencies and commands) to the appropriate RC2 packages / tools. (lot' sof renaming occured).
3. Add NET Core App target to applications. (This is the new framework you need to target + plus a package dependency, in order to build for .NET Core)
4. Add Net Standard target to Libraries. (This is the new framework TFM you need to target + package dependency, if you want build your library for .net standard)
 
NOTE: This tool will not refactor any code, or perform any backups. Those things will require you to do. Your mileage may vary.

# Thanks

A variety of sources are being used to build this tool.
1. Shaun's blog: https://wildermuth.com/2016/05/17/Converting-an-ASP-NET-Core-RC1-Project-to-RC2
2. @Pranavkm's tool that does project.json schema changes: https://github.com/pranavkm/FixProjectJsonWarnings


