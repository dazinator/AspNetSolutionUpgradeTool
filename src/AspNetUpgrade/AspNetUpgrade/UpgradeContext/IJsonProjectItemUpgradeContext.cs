using Newtonsoft.Json.Linq;

namespace AspNetUpgrade.UpgradeContext
{
    public interface IJsonProjectItemUpgradeContext
    {
        JObject ProjectItemJsonObject { get; set; }


        string Name();

    }
}