using System.IO;
using System.Text;
using AspNetUpgrade.Upgrader;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AspNetUpgrade.Tests
{
    public class TestJsonProjectUpgradeContext : JsonProjectUpgradeContext
    {
        private readonly string _jsonContents;

        private StringBuilder _modifiedJsonContents;

        public TestJsonProjectUpgradeContext(string jsonContents)
        {
            _jsonContents = jsonContents;
            _modifiedJsonContents = new StringBuilder();

            using (var streamReader = new StringReader(_jsonContents))
            {
                using (JsonTextReader reader = new JsonTextReader(streamReader))
                {
                    JsonObject = JObject.Load(reader);
                }
            }

        }

  
        public override void SaveChanges()
        {
            using (var writer = new StringWriter(_modifiedJsonContents))
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
        }

        public string ModifiedJsonContents { get { return _modifiedJsonContents.ToString(); } }


    }
}