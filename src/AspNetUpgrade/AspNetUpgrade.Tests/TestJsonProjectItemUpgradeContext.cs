using System.IO;
using System.Text;
using AspNetUpgrade.UpgradeContext;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AspNetUpgrade.Tests
{
    public class TestJsonProjectItemUpgradeContext : BaseJsonProjectItemUpgradeContext
    {
        private readonly string _name;
        private readonly string _jsonContents;
        private StringBuilder _modifiedJsonContents;

        public TestJsonProjectItemUpgradeContext(string name, string jsonContents)
        {
            _name = name;
            _jsonContents = jsonContents;
            _modifiedJsonContents = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(jsonContents))
            {
                using (var streamReader = new StringReader(_jsonContents))
                {
                    using (JsonTextReader reader = new JsonTextReader(streamReader))
                    {
                        ProjectItemJsonObject = JObject.Load(reader);
                    }
                }
            }
          
        }


        public override JObject ProjectItemJsonObject { get; set; }
        public override string Name()
        {
            return _name;
        }

        public override void SaveChanges()
        {
            using (var writer = new StringWriter(_modifiedJsonContents))
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

      
        public string ModifiedProjectJsonContents { get { return _modifiedJsonContents.ToString(); } }

    }
}