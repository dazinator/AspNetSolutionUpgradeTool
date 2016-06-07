# AspNetRC1toRC2UpgradeTool

This tool upgrades an RC1 / Dnx solution to RC2 / Preview1.

```

AspNetUpgrade.exe --solutionDir "E:\\path\\to\\your\\solution"

```



1. Upgrades the `project.json` files, `.xproj` files, and `global.json` files to the new schema's versions. This tool handles a tonne of things. so you don't have to.

2. Upgrades RC1 dependencies / packages (and commands) to the appropriate RC2 packages / tools. (lot's of renaming occured).

This tool *can* also upgrade Applications and Libraries to target `netcoreapp1.0` / `netstandard1.5` but I haven't enabled that feature by default even though it works.. could possible introduce a command line option to turn this on if people think it is useful.

NOTE: This tool will not refactor any code, or perform any backups. Those things will require you to do. Your mileage may vary.

# Thanks

A variety of sources are being used to build this tool:

1. Shaun's blog: https://wildermuth.com/2016/05/17/Converting-an-ASP-NET-Core-RC1-Project-to-RC2
2. @Pranavkm's tool that does project.json schema changes: https://github.com/pranavkm/FixProjectJsonWarnings


