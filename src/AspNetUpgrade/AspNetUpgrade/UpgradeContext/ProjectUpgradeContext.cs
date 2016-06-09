using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AspNetUpgrade.UpgradeContext
{
    public class ProjectUpgradeContext : BaseProjectUpgradeContext
    {
        private readonly FileInfo _projectJsonFileInfo;
        private FileInfo _xprojFileInfo;
        // private readonly StringBuilder _fileContents = new StringBuilder();

        public ProjectUpgradeContext(FileInfo projectJsonFile)
        {
            _projectJsonFileInfo = projectJsonFile;
            LoadProjectJsonFile();

            LoadVsProjectFile();

            CsharpFiles = new List<BaseCsharpFileUpgradeContext>();
            LoadCsharpFiles();

        }

        private void LoadVsProjectFile()
        {
            // check for xproj file in same dir and load.
            Microsoft.Build.Evaluation.ProjectCollection collection = new Microsoft.Build.Evaluation.ProjectCollection();
            var xprojFiles = _projectJsonFileInfo.Directory.GetFiles("*.xproj", SearchOption.TopDirectoryOnly);
            if (xprojFiles.Length == 1)
            {
                _xprojFileInfo = xprojFiles[0];
                Microsoft.Build.Evaluation.Project project = new Microsoft.Build.Evaluation.Project(_xprojFileInfo.FullName, null, null, collection, Microsoft.Build.Evaluation.ProjectLoadSettings.IgnoreMissingImports);
            }

        }

        public override void SaveChanges()
        {
            using (var writer = new StreamWriter(_projectJsonFileInfo.FullName))
            {
                using (var jsonWriter = new JsonTextWriter(writer))
                {
                    jsonWriter.Formatting = Formatting.Indented;
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(jsonWriter, JsonObject);
                    jsonWriter.Flush();
                    writer.Flush();
                }
            }

            VsProjectFile?.Save(_xprojFileInfo.FullName);

            foreach (var csharpFile in CsharpFiles)
            {
                csharpFile.SaveChanges();
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
                    JsonObject = JObject.Load(reader);
                }
            }
        }

        private void LoadCsharpFiles()
        {
            // get csharp files in project directory.
            var csharpFiles = _projectJsonFileInfo.Directory.GetFiles("*.cs", SearchOption.AllDirectories);
            var csharpFileUpgrades = csharpFiles.Select(file => new CsharpFileUpgradeContext(file)).ToList();
            this.CsharpFiles.AddRange(csharpFileUpgrades);
        }


    }
}