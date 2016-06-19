using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Build.Evaluation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AspNetUpgrade.UpgradeContext
{
    public class ProjectUpgradeContext : BaseProjectUpgradeContext
    {
        private readonly FileInfo _projectJsonFileInfo;
        private FileInfo _launchSettingsFileInfo;
        private FileInfo _xprojFileInfo;
        // private readonly StringBuilder _fileContents = new StringBuilder();

        public ProjectUpgradeContext(FileInfo projectJsonFile)
        {
            _projectJsonFileInfo = projectJsonFile;
            var projectDir = _projectJsonFileInfo.Directory;
            LoadProjectJsonFile();
            LoadLaunchSettingsJson(projectDir);
            LoadVsProjectFile(projectDir);
            CsharpFiles = new List<BaseCsharpFileUpgradeContext>();
            JsonFiles = new List<BaseJsonProjectItemUpgradeContext>();
            LoadCsharpFiles(projectDir);
            LoadJsonFiles(projectDir);
        }

        private void LoadVsProjectFile(DirectoryInfo projectDir)
        {
            // check for xproj file in same dir and load.
            Microsoft.Build.Evaluation.ProjectCollection collection = new Microsoft.Build.Evaluation.ProjectCollection();
            var xprojFiles = projectDir.GetFiles("*.xproj", SearchOption.TopDirectoryOnly);
            if (xprojFiles.Length == 1)
            {
                _xprojFileInfo = xprojFiles[0];
                Microsoft.Build.Evaluation.Project project = new Microsoft.Build.Evaluation.Project(_xprojFileInfo.FullName, null, null, collection, Microsoft.Build.Evaluation.ProjectLoadSettings.IgnoreMissingImports);
                this.VsProjectFile = project;
            }

        }

        private void LoadLaunchSettingsJson(DirectoryInfo projectDir)
        {
            _launchSettingsFileInfo = projectDir.GetFiles("launchSettings.json", SearchOption.AllDirectories).FirstOrDefault();
            if (_launchSettingsFileInfo != null)
            {
                if (_launchSettingsFileInfo.Exists)
                {
                    // _launchSettingsFileInfo = new FileInfo(globalJsonFile);
                    using (var streamReader = new StreamReader(_launchSettingsFileInfo.FullName))
                    {
                        using (JsonTextReader reader = new JsonTextReader(streamReader))
                        {
                            LaunchSettingsObject = JObject.Load(reader);
                        }
                    }
                }
            }
            //  var globalJsonFile = Path.Combine(solutionDir.FullName, "launchSettings.json");


        }

        public override JObject ProjectJsonObject { get; set; }
        public override JObject LaunchSettingsObject { get; set; }
        public override Project VsProjectFile { get; set; }

        public override void SaveChanges()
        {
            using (var writer = new StreamWriter(_projectJsonFileInfo.FullName))
            {
                using (var jsonWriter = new JsonTextWriter(writer))
                {
                    jsonWriter.Formatting = Formatting.Indented;
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(jsonWriter, ProjectJsonObject);
                    jsonWriter.Flush();
                    writer.Flush();
                }
            }

            if (LaunchSettingsObject != null)
            {
                using (var writer = new StreamWriter(_launchSettingsFileInfo.FullName))
                {
                    using (var jsonWriter = new JsonTextWriter(writer))
                    {
                        jsonWriter.Formatting = Formatting.Indented;
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(jsonWriter, LaunchSettingsObject);
                        jsonWriter.Flush();
                        writer.Flush();
                    }
                }
            }

            VsProjectFile?.Save(_xprojFileInfo.FullName);

            foreach (var csharpFile in CsharpFiles)
            {
                csharpFile.SaveChanges();
            }

            foreach (var jsonFile in JsonFiles)
            {
                jsonFile.SaveChanges();
            }
        }

        public override string ProjectName()
        {
            if (VsProjectFile != null)
            {
                var path = VsProjectFile.FullPath;
                var name = System.IO.Path.GetFileNameWithoutExtension(path);
                return name;
            }
            else
            {
                //just use project dir name.
                var projectFolder = _projectJsonFileInfo.Directory;
                if (projectFolder != null)
                {
                    var projectFolderName = projectFolder.Name;
                    return projectFolderName;
                }
                else
                {
                    // no idea.
                    return "ProjectName";
                }

            }
        }

        public override string ToString()
        {
            return _projectJsonFileInfo.FullName;
        }

        private void LoadProjectJsonFile()
        {
            using (var streamReader = new StreamReader(_projectJsonFileInfo.FullName))
            {
                using (JsonTextReader reader = new JsonTextReader(streamReader))
                {
                    ProjectJsonObject = JObject.Load(reader);
                }
            }
        }

        private void LoadCsharpFiles(DirectoryInfo projectDir)
        {
            // get csharp files in project directory.
            var csharpFiles = projectDir.GetFiles("*.cs", SearchOption.AllDirectories);
            var csharpFileUpgrades = csharpFiles.Select(file => new CsharpFileUpgradeContext(file)).ToList();
            this.CsharpFiles.AddRange(csharpFileUpgrades);
        }

        private void LoadJsonFiles(DirectoryInfo projectDir)
        {
            // Loads json files in project directory - that are not project.json. i.e appsettings.*.json etc.
            var jsonFiles = projectDir.GetFiles("*.json", SearchOption.TopDirectoryOnly);
            var jsonFileUpgradeContexts = jsonFiles.Where(a => a.Name.ToLowerInvariant() != "project.json").Select(file => new JsonProjectItemUpgradeContext(file)).ToList();
            this.JsonFiles.AddRange(jsonFileUpgradeContexts);
        }


    }
}