using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AspNetUpgrade.Upgrader
{
    public class SolutionUpgradeContext : BaseSolutionUpgradeContext
    {
        private FileInfo _globalJsonFileInfo;
        private readonly DirectoryInfo _solutionDir;
        
        public SolutionUpgradeContext(DirectoryInfo solutionDir)
        {
            _solutionDir = solutionDir;
            LoadGlobalJson(solutionDir);
            LoadProjects(solutionDir);
        }

        private void LoadGlobalJson(DirectoryInfo solutionDir)
        {
            var globalJsonFile = Path.Combine(_solutionDir.FullName, "global.json");
            if (File.Exists(globalJsonFile))
            {
                _globalJsonFileInfo = new FileInfo(globalJsonFile);
                using (var streamReader = new StreamReader(_globalJsonFileInfo.FullName))
                {
                    using (JsonTextReader reader = new JsonTextReader(streamReader))
                    {
                        GlobalJsonObject = JObject.Load(reader);
                    }
                }
            }
            
        }

        public void LoadProjects(DirectoryInfo solutionDir)
        {
            var projFiles = solutionDir.GetFiles("project.json", SearchOption.AllDirectories);
            var projects = projFiles.Select(file => new JsonProjectFileUpgradeContext(file)).ToList();
            this.Projects.AddRange(projects);
        }

        /// <summary>
        /// Saves the upgraded solution, including all modified project files, to disk.
        /// </summary>
        public override void SaveChanges()
        {
            foreach (var project in Projects)
            {
                project.SaveChanges();
            }

            using (var writer = new StreamWriter(_globalJsonFileInfo.FullName))
            {
                using (var jsonWriter = new JsonTextWriter(writer))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(jsonWriter, GlobalJsonObject);
                    jsonWriter.Flush();
                    writer.Flush();
                }
            }
        }

    }
}