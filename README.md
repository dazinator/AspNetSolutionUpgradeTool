# AspNetRC1toRC2UpgradeTool
A utility that can be used to upgrade ASP.NET 5 RC1 based projects, to ASP.NET Core RC2.

Based upon the steps that Shaun Wildermuth blogged about manually. This tool attempts to automate the steps that are easy to automate. You will still be left with some of your own manual steps to take - this tool won't do everything for you, but I am trying to add as much as possible.

https://wildermuth.com/2016/05/17/Converting-an-ASP-NET-Core-RC1-Project-to-RC2

# How does it work?

It's an exe, you call it with some arguments pointing to your project.json files. It will loop through them and apply upgrade `Actions` to upgrade the json in memory. Once all actions have been applied, it will save the modified project.json files to disk again overwriting the old ones. TAKE A BACKUP BEFORE RUNNING THIS TOOL.


# What Changes does it make?

1. Updates the "frameworks" section. If targeting `dnx451` then will be changed to `net452`. If targeting dnxcore50, that will be changed to:

```
"frameworks": {
  "netcoreapp1.0": {
    "imports": [
      "dotnet5.6",
      "dnxcore50",
      "portable-net45+win8"
    ]
  }
}
```
