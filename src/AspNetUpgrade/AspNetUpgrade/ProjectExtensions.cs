using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Build.Construction;
using Newtonsoft.Json.Linq;

namespace AspNetUpgrade
{
    public static class ProjectExtensions
    {
       
        public static void AddPropertyValue(this Microsoft.Build.Evaluation.Project project, string propertyGroupLabel, string propertyName, string value)
        {
            // _Logger.LogInfo(string.Format("Project Dir is: {0}", projectDir));
            var existingPropGroup = project.Xml.PropertyGroups.Cast<ProjectPropertyGroupElement>().FirstOrDefault(i => i.Label.ToLowerInvariant() == propertyGroupLabel.ToLowerInvariant());
            if (existingPropGroup != null)
            {
                var existingProp = existingPropGroup.Properties.Cast<ProjectPropertyElement>().FirstOrDefault(i => i.Name.ToLowerInvariant() == propertyName.ToLowerInvariant());
                if (existingProp != null)
                {
                    existingProp.Value = value;
                }
                else
                {
                    existingPropGroup.AddProperty(propertyName, value);
                }
            }

        }

        public static void UpdatePropertyValue(this Microsoft.Build.Evaluation.Project project, string propertyName, string value)
        {
            // _Logger.LogInfo(string.Format("Project Dir is: {0}", projectDir));
            var existingProp = project.Xml.Properties.Cast<ProjectPropertyElement>().FirstOrDefault(i => i.Name.ToLowerInvariant() == propertyName.ToLowerInvariant());
            if (existingProp != null)
            {
                existingProp.Value = value;
            }
        }

        public static void UpdateImport(this Microsoft.Build.Evaluation.Project project, string oldImportProjectName, string newImportProjectName)
        {
            // Ensure import is present, replace existing if found.
            var fileName = Path.GetFileName(oldImportProjectName);
            var existingImport = project.Xml.Imports.Cast<ProjectImportElement>().FirstOrDefault(i => i.Project.EndsWith(fileName));
            if (existingImport != null)
            {
                existingImport.Project = newImportProjectName;
            }

        }
    }
}