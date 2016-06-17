using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AspNetUpgrade.UpgradeContext;
using Microsoft.Build.Evaluation;
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

        private StringBuilder _modifiedLaunchSettingsContents;

        public TestJsonBaseProjectUpgradeContext(string jsonContents, Microsoft.Build.Evaluation.Project project, string launchSettingsJson, string[] appSettingsFileNames = null)
        {

            _jsonContents = jsonContents;
            _modifiedJsonContents = new StringBuilder();
            _modifiedXprojContents = new StringBuilder();
            _modifiedLaunchSettingsContents = new StringBuilder();

            this.JsonFiles = new List<BaseJsonProjectItemUpgradeContext>();
            if (appSettingsFileNames != null)
            {
                foreach (var jsonFile in appSettingsFileNames)
                {
                    this.JsonFiles.Add(new TestJsonProjectItemUpgradeContext(jsonFile, string.Empty));
                }
            }
          

            VsProjectFile = project;

            using (var streamReader = new StringReader(_jsonContents))
            {
                using (JsonTextReader reader = new JsonTextReader(streamReader))
                {
                    ProjectJsonObject = JObject.Load(reader);
                }
            }
            if (!string.IsNullOrWhiteSpace(launchSettingsJson))
            {
                using (var streamReader = new StringReader(launchSettingsJson))
                {
                    using (JsonTextReader reader = new JsonTextReader(streamReader))
                    {
                        LaunchSettingsObject = JObject.Load(reader);
                    }
                }
            }


        }


        public override JObject ProjectJsonObject { get; set; }
        public override JObject LaunchSettingsObject { get; set; }
        public override Project VsProjectFile { get; set; }

        public override void SaveChanges()
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

            if (LaunchSettingsObject != null)
            {
                using (var writer = new StringWriter(_modifiedLaunchSettingsContents))
                {
                    using (var jsonWriter = new JsonTextWriter(writer))
                    {
                        jsonWriter.Formatting = Formatting.Indented;
                        JsonSerializer serializer = new JsonSerializer();

                        serializer.Serialize(jsonWriter, LaunchSettingsObject);
                        jsonWriter.Flush();
                        writer.Flush();
                    }
                }

            }


        }

        public override string ProjectName()
        {
            return "TestProject";
        }

        public string ModifiedProjectJsonContents { get { return _modifiedJsonContents.ToString(); } }

        public string ModifiedLaunchSettingsJsonContents { get { return _modifiedLaunchSettingsContents.ToString(); } }

    }
}