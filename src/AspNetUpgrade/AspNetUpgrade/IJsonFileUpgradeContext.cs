using Newtonsoft.Json.Linq;

namespace AspNetUpgrade
{
    public interface IJsonFileUpgradeContext
    {
        // TextReader CreateReader();

        JObject ProjectJsonObject { get; set; }

        void SaveChanges();
    }
}