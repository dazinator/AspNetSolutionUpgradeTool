using Newtonsoft.Json.Linq;

namespace AspNetUpgrade.Upgrader
{
    public interface IJsonFileUpgradeContext
    {
        // TextReader CreateReader();

        JObject ProjectJsonObject { get; set; }

        void SaveChanges();
    }
}