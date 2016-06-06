using Newtonsoft.Json.Linq;

namespace AspNetUpgrade.UpgradeContext
{
    public interface IProjectUpgradeContext
    {
        JObject JsonObject { get; set; }
        Microsoft.Build.Evaluation.Project VsProjectFile { get; set; }
        void SaveChanges();
        ProjectJsonWrapper ToProjectJsonWrapper();


    }
}