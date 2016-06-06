using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace AspNetUpgrade.Upgrader
{
    public interface ISolutionUpgradeContext
    {
        JObject GlobalJsonObject { get; set; }
        void SaveChanges();

        List<JsonProjectUpgradeContext> Projects { get; }

        void BeginUpgrade(Action callback);
    }
}