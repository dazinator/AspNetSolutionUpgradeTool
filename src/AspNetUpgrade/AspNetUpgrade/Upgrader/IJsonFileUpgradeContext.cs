using Newtonsoft.Json.Linq;

namespace AspNetUpgrade.Upgrader
{
    public interface IJsonFileUpgradeContext
    {
        // TextReader CreateReader();

        JObject JsonObject { get; set; }

        void SaveChanges();
    }
}