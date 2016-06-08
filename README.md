# AspNetRC1toRC2UpgradeTool

This tool upgrades an RC1 / Dnx solution to RC2 / Preview1.

```

AspNetUpgrade.exe --solutionDir "E:\\path\\to\\your\\solution"

```



1. Upgrades the `project.json` files, `.xproj` files, and `global.json` files to the new schema's versions.
2. Uses Roslyn, to correct some old RC1 using statements in csharp code files for you. (Based on a common list I have put together from various sources and via trial and error on my own solution)
3. Upgrades RC1 based NuGet packages, (and some commands) to the appropriate RC2 packages / tools. (lot's of renaming occured).

NOTE: This tool only does some simple code refactoring at present - i.e using statement rewrites, all the rest is still left to you, however as this tool has access to the full `SyntaxTree` it's fairly easy to add new analysis / refactorings into the tool if you know what you are doing with Roslyn ;)

Your mileage may vary.

# Thanks

A variety of sources are being used to build this tool:

1. Shaun's blog: https://wildermuth.com/2016/05/17/Converting-an-ASP-NET-Core-RC1-Project-to-RC2
2. @Pranavkm's tool that does project.json schema changes: https://github.com/pranavkm/FixProjectJsonWarnings


