using System.IO;
using System.Xml;
using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;

namespace AspNetUpgrade.Tests.XProj
{
    public static class VsProjectFactory
    {
        public static Project LoadTestProject(string xprojContents)
        {
            Microsoft.Build.Evaluation.ProjectCollection collection = new Microsoft.Build.Evaluation.ProjectCollection();
            //Microsoft.Build.Evaluation.Project x== new Microsoft.Build.Evaluation.Project()
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
    }
}