using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AspNetUpgrade.Upgrader
{
    public class JsonProjectFileUpgradeContext : JsonProjectUpgradeContext
    {
        private readonly FileInfo _fileInfo;
        // private readonly StringBuilder _fileContents = new StringBuilder();

        public JsonProjectFileUpgradeContext(FileInfo fileInfo)
        {
            _fileInfo = fileInfo;
            using (var streamReader = new StreamReader(_fileInfo.FullName))
            {
                using (JsonTextReader reader = new JsonTextReader(streamReader))
                {
                    JsonObject = JObject.Load(reader);
                }
            }
        }

        public override void SaveChanges()
        {
            using (var writer = new StreamWriter(_fileInfo.FullName))
            {
                using (var jsonWriter = new JsonTextWriter(writer))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(jsonWriter, JsonObject);
                    jsonWriter.Flush();
                    writer.Flush();
                }
            }
        }

    }
}