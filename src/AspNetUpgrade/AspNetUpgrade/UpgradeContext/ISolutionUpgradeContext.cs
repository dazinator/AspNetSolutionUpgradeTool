using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace AspNetUpgrade.UpgradeContext
{
    public interface ISolutionUpgradeContext
    {
        JObject GlobalJsonObject { get; set; }
        void SaveChanges();
        List<BaseProjectUpgradeContext> Projects { get; }
        void BeginUpgrade(Action callback);
    }
}