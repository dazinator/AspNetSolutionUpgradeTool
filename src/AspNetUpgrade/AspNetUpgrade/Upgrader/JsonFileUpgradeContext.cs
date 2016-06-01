using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AspNetUpgrade.Upgrader
{
    public class JsonFileUpgradeContext : IJsonFileUpgradeContext
    {
        private readonly FileInfo _fileInfo;
        // private readonly StringBuilder _fileContents = new StringBuilder();

        public JsonFileUpgradeContext(FileInfo fileInfo)
        {
            _fileInfo = fileInfo;
            using (var streamReader = CreateReader())
            {
                using (JsonTextReader reader = new JsonTextReader(streamReader))
                {
                    ProjectJsonObject = JObject.Load(reader);
                }
            }
        }

        public TextReader CreateReader()
        {
            return new StreamReader(_fileInfo.FullName);
        }

        // public TargetFrameworkKind TargetFrameworkKind { get; set; }


        public JObject ProjectJsonObject { get; set; }


        public void SaveChanges()
        {
            using (var writer = new StreamWriter(_fileInfo.FullName))
            {
                using (var jsonWriter = new JsonTextWriter(writer))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(jsonWriter, ProjectJsonObject);
                    jsonWriter.Flush();
                    writer.Flush();
                }
            }
        }


    }
}