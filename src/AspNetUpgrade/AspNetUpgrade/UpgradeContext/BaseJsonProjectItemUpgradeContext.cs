using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Build.Evaluation;
using Newtonsoft.Json.Linq;

namespace AspNetUpgrade.UpgradeContext
{
    public abstract class BaseJsonProjectItemUpgradeContext : IJsonProjectItemUpgradeContext
    {

        private JObject _clone;

        public abstract JObject ProjectItemJsonObject { get; set; }

        public abstract void SaveChanges();
        
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
            _clone = (JObject)ProjectItemJsonObject.DeepClone();
        }

        private void RestoreClone()
        {
            if (_clone == null)
            {
                throw new InvalidOperationException("must call clone first");
            }
            ProjectItemJsonObject = _clone;
        }
      
        public abstract string Name();


    }


}