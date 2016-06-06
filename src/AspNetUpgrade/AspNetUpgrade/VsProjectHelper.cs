using System.IO;
using System.Text;
using System.Xml;
using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;

namespace AspNetUpgrade
{
    public static class VsProjectHelper
    {
        public static Project LoadTestProject(string xprojContents)
        {
            Microsoft.Build.Evaluation.ProjectCollection collection = new Microsoft.Build.Evaluation.ProjectCollection();
            using (var stringReader = new StringReader(xprojContents))
            {
                using (var reader = new XmlTextReader(stringReader))
                {
                    var root = ProjectRootElement.Create(reader, collection);
                    var project = new Project(root, null, null, collection, Microsoft.Build.Evaluation.ProjectLoadSettings.IgnoreMissingImports);
                    return project;
                }
            }

        }


        public static string ToString(Project project)
        {
            if (project != null)
            {
                var builder = new StringBuilder();
                using (var writer = new StringWriter(builder))
                {
                    project.Save(writer);
                    writer.Flush();
                }

                return builder.ToString();
            }

            return string.Empty;
        }
    }
}