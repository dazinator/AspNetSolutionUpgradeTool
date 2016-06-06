using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AspNetUpgrade.Upgrader
{
    public abstract class BaseSolutionUpgradeContext : ISolutionUpgradeContext
    {

        private JObject _globalJsonClone;

        public BaseSolutionUpgradeContext()
        {
            this.Projects = new List<JsonProjectUpgradeContext>();
        }

        public JObject GlobalJsonObject { get; set; }
        public abstract void SaveChanges();

        public List<JsonProjectUpgradeContext> Projects { get; }

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
            _globalJsonClone = (JObject)GlobalJsonObject.DeepClone();
        }

        private void RestoreClone()
        {
            if (_globalJsonClone == null)
            {
                throw new InvalidOperationException("must call clone first");
            }
            GlobalJsonObject = _globalJsonClone;
        }


    }
}