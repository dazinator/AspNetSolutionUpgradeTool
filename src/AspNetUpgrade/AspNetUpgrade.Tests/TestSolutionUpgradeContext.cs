using System.Collections.Generic;
using System.IO;
using System.Text;
using AspNetUpgrade.Upgrader;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AspNetUpgrade.Tests
{
    public class TestSolutionUpgradeContext : BaseSolutionUpgradeContext
    {
        private readonly string _jsonContents;

        private StringBuilder _modifiedJsonContents;

        public TestSolutionUpgradeContext(string globalJsonContents) : base()
        {
            _jsonContents = globalJsonContents;
            _modifiedJsonContents = new StringBuilder();
            using (var streamReader = new StringReader(_jsonContents))
            {
                using (JsonTextReader reader = new JsonTextReader(streamReader))
                {
                    GlobalJsonObject = JObject.Load(reader);
                }
            }
        }

        public override void SaveChanges()
        {

            foreach (var project in this.Projects)
            {
                project.SaveChanges();
            }

            using (var writer = new StringWriter(_modifiedJsonContents))
            {
                using (var jsonWriter = new JsonTextWriter(writer))
                {
                    jsonWriter.Formatting = Formatting.Indented;
                    JsonSerializer serializer = new JsonSerializer();

                    serializer.Serialize(jsonWriter, GlobalJsonObject);
                    jsonWriter.Flush();
                    writer.Flush();
                }
            }
        }

        public string ModifiedJsonContents { get { return _modifiedJsonContents.ToString(); } }





    }
}