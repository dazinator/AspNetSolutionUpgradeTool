# AspNetRC1toRC2UpgradeTool
A utility that can be used to upgrade ASP.NET 5 RC1 based projects, to ASP.NET Core RC2.

Based upon the steps that Shaun Wildermuth blogged about manually. This tool attempts to automate the steps that are easy to automate. You will still be left with some of your own manual steps to take - this tool won't do everything for you, but I am trying to add as much as possible.

https://wildermuth.com/2016/05/17/Converting-an-ASP-NET-Core-RC1-Project-to-RC2

# How does it work?

Note: It doesn't currently - as it's still in development - see issues! :)

It's an exe, you call it with some arguments pointing to your project.json files. It will loop through them and apply upgrade `Actions` to upgrade your project files in memory. Once all actions have been applied, it will save the modified files to disk again overwriting the old ones. MAKE SURE YOU TAKE A BACKUP BEFORE RUNNING THIS TOOL.


# What Changes does it make?

In a nutshell, this tool will update the `project.json` files and any `.xproj` files from RC1 format to an RC2 format. This means changes to use the `dotnet cli` tooling instead of `dnx` and also changing references to `RC1` based dependencies to be the appropriate `RC2` based dependencies / nuget packages.

There are a host of other changes that this tool will make. Will document them all before release.
