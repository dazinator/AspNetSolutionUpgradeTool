using System.IO;
using System.Text;
using AspNetUpgrade.UpgradeContext;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Formatting = Newtonsoft.Json.Formatting;

namespace AspNetUpgrade.Tests
{
    public class TestJsonBaseProjectUpgradeContext : BaseProjectUpgradeContext
    {
        private readonly string _jsonContents;

        private StringBuilder _modifiedJsonContents;

        private StringBuilder _modifiedXprojContents;

        public TestJsonBaseProjectUpgradeContext(string jsonContents, Microsoft.Build.Evaluation.Project project)
        {

            _jsonContents = jsonContents;
            _modifiedJsonContents = new StringBuilder();
            _modifiedXprojContents = new StringBuilder();

            VsProjectFile = project;

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

            if (VsProjectFile != null)
            {
                using (var writer = new StringWriter(_modifiedXprojContents))
                {
                    // using (var xmlWriter = new XmlTextWriter(writer))
                    // {
                    //  xmlWriter.Formatting = System.Xml.Formatting.Indented;
                    VsProjectFile.Save(writer);

                    // JsonSerializer serializer = new JsonSerializer();

                    // serializer.Serialize(jsonWriter, JsonObject);
                    writer.Flush();
                    //writer.Flush();
                    // }
                }
            }

        }

        public string ModifiedJsonContents { get { return _modifiedJsonContents.ToString(); } }

    }
}