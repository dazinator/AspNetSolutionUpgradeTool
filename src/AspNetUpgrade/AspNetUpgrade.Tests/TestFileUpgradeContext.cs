using System.IO;
using System.Text;
using AspNetUpgrade.Upgrader;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AspNetUpgrade.Tests
{
    public class TestFileUpgradeContext : IJsonFileUpgradeContext
    {
        private readonly string _jsonContents;

        private StringBuilder _modifiedJsonContents;

        public TestFileUpgradeContext(string jsonContents)
        {
            _jsonContents = jsonContents;
            _modifiedJsonContents = new StringBuilder();

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
            return new StringReader(_jsonContents);
        }

        public JObject ProjectJsonObject { get; set; }

        public void SaveChanges()
        {
            using (var writer = new StringWriter(_modifiedJsonContents))
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
        }

        public string ModifiedJsonContents { get { return _modifiedJsonContents.ToString(); } }


    }
}