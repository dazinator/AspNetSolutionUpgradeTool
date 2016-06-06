using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace AspNetUpgrade.UpgradeContext
{
    public abstract class BaseSolutionUpgradeContext : ISolutionUpgradeContext
    {

        private JObject _globalJsonClone;

        public BaseSolutionUpgradeContext()
        {
            this.Projects = new List<BaseProjectUpgradeContext>();
        }

        public JObject GlobalJsonObject { get; set; }
        public abstract void SaveChanges();

        public List<BaseProjectUpgradeContext> Projects { get; }

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