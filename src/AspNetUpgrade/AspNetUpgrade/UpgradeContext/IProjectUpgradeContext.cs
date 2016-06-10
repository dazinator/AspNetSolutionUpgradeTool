using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json.Linq;

namespace AspNetUpgrade.UpgradeContext
{
    public interface IProjectUpgradeContext
    {
        JObject ProjectJsonObject { get; set; }
        JObject LaunchSettingsObject { get; set; }
        Microsoft.Build.Evaluation.Project VsProjectFile { get; set; }
        void SaveChanges();
        ProjectJsonWrapper ToProjectJsonWrapper();

        List<BaseCsharpFileUpgradeContext> CsharpFiles { get; set; }

        string ProjectName();

    }
}