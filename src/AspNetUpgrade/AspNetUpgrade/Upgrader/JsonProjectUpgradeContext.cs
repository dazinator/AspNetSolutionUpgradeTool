using System;
using Newtonsoft.Json.Linq;

namespace AspNetUpgrade.Upgrader
{
    public abstract class JsonProjectUpgradeContext : IJsonProjectUpgradeContext
    {
        public JObject JsonObject { get; set; }

        public abstract void SaveChanges();

        public ProjectJsonWrapper ToProjectJsonWrapper()
        {
            return new ProjectJsonWrapper(JsonObject);
        }

        private JObject _clone;

        public void BeginUpgrade(Action callback)
        {
            try
            {
                Clone();
                callback();
            }
            catch (Exception e)
            {
                // if there is an error during the upgrade, restore the file to snapshot.
                RestoreClone();
                throw;
            }
        }

        private void Clone()
        {
            _clone = (JObject)JsonObject.DeepClone();
        }

        private void RestoreClone()
        {
            if (_clone == null)
            {
                throw new InvalidOperationException("must call clone first");
            }
            JsonObject = _clone;
        }
    }
}