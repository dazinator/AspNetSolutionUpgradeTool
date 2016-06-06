using Newtonsoft.Json.Linq;

namespace AspNetUpgrade.Upgrader
{
    public class ProjectJsonWrapper
    {
        JObject JsonObject { get; set; }

        public ProjectJsonWrapper(JObject jsonObject)
        {
            JsonObject = jsonObject;
        }

        /// <summary>
        /// Returns the project type.
        /// </summary>
        /// <returns></returns>
        public ProjectType GetProjectType()
        {
            // if the project.json has emitEntryPoint = true then this is an application, otherwise its a library.
            var compilationOpions = JsonObject["compilationOptions"] ?? JsonObject["buildOptions"];
            if (compilationOpions != null)
            {
                var emitEntryPoint = compilationOpions["emitEntryPoint"];
                if (emitEntryPoint != null)
                {
                    if (emitEntryPoint.ToString().ToLowerInvariant() == "true")
                    {
                        // application
                        return ProjectType.Application;
                    }
                }
            }

            return ProjectType.Library;
        }

        public bool HasDependency(string dependencyName)
        {
            bool hasDependency = JsonObject["dependencies"]?[dependencyName] != null;
            return hasDependency;
        }

        public bool IsMvcProject()
        {
            return HasDependency("Microsoft.AspNetCore.Mvc")
                   || HasDependency("Microsoft.AspNet.Mvc");
        }

    }
}