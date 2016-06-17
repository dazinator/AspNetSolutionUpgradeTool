using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AspNetUpgrade.UpgradeContext
{
    public class JsonProjectItemUpgradeContext : BaseJsonProjectItemUpgradeContext
    {
        private readonly FileInfo _JsonFileInfo;

        public JsonProjectItemUpgradeContext(FileInfo jsonFile)
        {
            _JsonFileInfo = jsonFile;
            LoadJsonFile();
        }

        private void LoadJsonFile()
        {
            using (var streamReader = new StreamReader(_JsonFileInfo.FullName))
            {
                using (JsonTextReader reader = new JsonTextReader(streamReader))
                {
                    ProjectItemJsonObject = JObject.Load(reader);
                }
            }
        }

        public override JObject ProjectItemJsonObject { get; set; }

        public override void SaveChanges()
        {
            using (var writer = new StreamWriter(_JsonFileInfo.FullName))
            {
                using (var jsonWriter = new JsonTextWriter(writer))
                {
                    jsonWriter.Formatting = Formatting.Indented;
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(jsonWriter, ProjectItemJsonObject);
                    jsonWriter.Flush();
                    writer.Flush();
                }
            }
        }

        public override string Name()
        {
            var path = _JsonFileInfo.FullName;
            var name = System.IO.Path.GetFileName(path);
            return name;
        }

        public override string ToString()
        {
            return _JsonFileInfo.FullName;
        }

    }
}