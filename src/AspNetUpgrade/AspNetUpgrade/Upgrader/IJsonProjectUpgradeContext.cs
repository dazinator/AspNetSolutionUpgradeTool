using Newtonsoft.Json.Linq;

namespace AspNetUpgrade.Upgrader
{
    public interface IJsonProjectUpgradeContext
    {
        // TextReader CreateReader();

        JObject JsonObject { get; set; }

        Microsoft.Build.Evaluation.Project VsProjectFile { get; set; }
        

        void SaveChanges();

        // bool IsWebApplicationProject();
        ProjectJsonWrapper ToProjectJsonWrapper();


    }
}