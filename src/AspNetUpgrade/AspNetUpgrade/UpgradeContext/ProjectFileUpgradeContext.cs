using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AspNetUpgrade.UpgradeContext
{
    public class BaseProjectFileUpgradeContext : BaseProjectUpgradeContext
    {
        private readonly FileInfo _projectJsonFileInfo;
        private FileInfo _xprojFileInfo;
        // private readonly StringBuilder _fileContents = new StringBuilder();

        public BaseProjectFileUpgradeContext(FileInfo jsonProjectFile)
        {
            _projectJsonFileInfo = jsonProjectFile;
            using (var streamReader = new StreamReader(_projectJsonFileInfo.FullName))
            {
                using (JsonTextReader reader = new JsonTextReader(streamReader))
                {
                    JsonObject = JObject.Load(reader);
                }
            }
            LoadVsProjectFile();
        }

        public virtual void LoadVsProjectFile()
        {
            // check for vsproj file in same dir and load.

           // Microsoft.Build.Evaluation.Project project

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
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(jsonWriter, JsonObject);
                    jsonWriter.Flush();
                    writer.Flush();
                }
            }
          
            VsProjectFile?.Save(_xprojFileInfo.FullName);
        }

        public override string ToString()
        {
            return _projectJsonFileInfo.FullName;
        }
    }
}