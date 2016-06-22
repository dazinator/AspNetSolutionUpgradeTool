using System.Linq;
using AspNetUpgrade.Model;
using Newtonsoft.Json.Linq;

namespace AspNetUpgrade.UpgradeContext
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

        public bool HasFramework(string targetFrameworkMoniker)
        {
            bool hasDependency = JsonObject["frameworks"]?[targetFrameworkMoniker] != null;
            return hasDependency;
        }

        public bool IsMvcProject()
        {
            return HasDependency("Microsoft.AspNetCore.Mvc")
                   || HasDependency("Microsoft.AspNet.Mvc");
        }

        public void AddNetCoreAppFramework(string netCoreAppVersion, string netCoreAppTfm)
        {
            var prop = BuildNetCoreAppProperty(netCoreAppVersion, netCoreAppTfm);
            JObject frameworks = (JObject)JsonObject.GetOrAddProperty("frameworks", null);
           frameworks.Add(prop);
        }

        public void AddNetStandardFramework(string netStandardLibraryVersion, string netStandardTfm)
        {

            string[] imports = new string[] { "dnxcore50", "portable-net45+win8" };

            JObject projectJsonObject = JsonObject;
            var frameworks = projectJsonObject.GetOrAddProperty("frameworks", null);
            var netStadardTfm = frameworks.GetOrAddProperty(netStandardTfm, null);
            var netStandardImports = netStadardTfm["imports"];

            JArray importsArray = new JArray();
            foreach (var import in imports)
            {
                importsArray.Add(import);
            }

            if (netStandardImports == null)
            {
                netStadardTfm["imports"] = importsArray;
            }

            JObject netStadardDependencies = netStadardTfm.GetOrAddProperty("dependencies", null);
            netStadardDependencies["NETStandard.Library"] = netStandardLibraryVersion;

        }

        public void IncludeInCopyToOutput(JArray itemsToInclude)
        {
            var buildOptions = JsonObject.GetOrAddProperty("buildOptions", null);
            //  var existingCopyToOutput = buildOptions[copyToOutputArray];
            var copyToOutput = buildOptions.Property("copyToOutput");
            if (copyToOutput != null)
            {
                if (copyToOutput.Value is JValue)
                {
                    // promote to include array.
                    var jvalue = (JValue)copyToOutput.Value;
                    //jvalue.Remove();

                    if (!itemsToInclude.Contains(jvalue.Value))
                    {
                        itemsToInclude.Add(jvalue.Value);
                    }

                    buildOptions["copyToOutput"]["include"] = itemsToInclude;
                }
                else if (copyToOutput.Value is JArray)
                {
                    // add to existing array.
                    var existingArray = (JArray)copyToOutput.Value;
                    IncludeInArray(existingArray, itemsToInclude);
                }
                else if (copyToOutput.Value is JObject)
                {
                    // check for an include property.

                    var copyToOutputObject = (JObject)copyToOutput.Value;
                    var includeProperty = copyToOutputObject.Property("include");

                    if (includeProperty == null)
                    {
                        buildOptions["copyToOutput"]["include"] = itemsToInclude;
                    }
                    else if (includeProperty.Value is JValue)
                    {
                        // single value, promote to an array
                        var jvalue = (JValue)includeProperty.Value;
                        if (!itemsToInclude.Contains(jvalue.Value))
                        {
                            itemsToInclude.Add(jvalue.Value);
                        }
                        buildOptions["copyToOutput"]["include"] = itemsToInclude;
                    }
                    else if (includeProperty.Value is JArray)
                    {
                        // already array, merge
                        var propArray = (JArray)includeProperty.Value;
                        IncludeInArray(propArray, itemsToInclude);
                    }
                }
            }
            else
            {
                buildOptions["copyToOutput"] = new JObject();
                buildOptions["copyToOutput"]["include"] = itemsToInclude;
            }

        }

        private void IncludeInArray(JArray existingArray, JArray includeValues)
        {
            foreach (var val in includeValues)
            {
                if (!existingArray.Contains(val))
                {
                    existingArray.Add(val);
                }
            }
        }
        private JProperty BuildNetCoreAppProperty(string netCoreAppVersion, string netCoreAppTfm)
        {
            // think these imports may only be needed temporarily for RC2 and may be disappearing after RC2?
            // applications should depend upon netcoreapp1.0
            JArray importsArray = new JArray();
            importsArray.Add("dotnet5.6");
            importsArray.Add("dnxcore50");
            importsArray.Add("portable-net45+win8");

            var importsProperty = new JProperty("imports", importsArray);
            var netCoreAppObj = new JObject(importsProperty);

            // Add the netCoreAppDependency dependency.
            // as per: https://github.com/dotnet/cli/issues/3171
            JObject netCoreAppDependencyObject = new JObject();
            netCoreAppDependencyObject.Add(new JProperty("version", netCoreAppVersion));
            netCoreAppDependencyObject.Add(new JProperty("type", "platform"));


            var deps = netCoreAppObj.GetOrAddProperty("dependencies", importsProperty);
            deps["Microsoft.NETCore.App"] = netCoreAppDependencyObject;

            return new JProperty(netCoreAppTfm, netCoreAppObj);

        }

    }
}