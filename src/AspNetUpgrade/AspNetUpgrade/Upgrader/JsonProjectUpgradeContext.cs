using System;
using System.IO;
using System.Text;
using Microsoft.Build.Evaluation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AspNetUpgrade.Upgrader
{
    public abstract class JsonProjectUpgradeContext : IJsonProjectUpgradeContext
    {

        private JObject _clone;
        private StringBuilder _xprojBackup;

        public JObject JsonObject { get; set; }

        public Project VsProjectFile { get; set; }

        public abstract void SaveChanges();

        public ProjectJsonWrapper ToProjectJsonWrapper()
        {
            return new ProjectJsonWrapper(JsonObject);
        }

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

            if (VsProjectFile != null)
            {
                _xprojBackup = new StringBuilder();
                using (var writer = new StringWriter(_xprojBackup))
                {
                    VsProjectFile.Xml.Save(writer);
                    writer.Flush();
                }
            }

        }

        private void RestoreClone()
        {
            if (_clone == null)
            {
                throw new InvalidOperationException("must call clone first");
            }
            JsonObject = _clone;

            if (VsProjectFile != null)
            {
                VsProjectFile = VsProjectHelper.LoadTestProject(_xprojBackup.ToString());
            }

        }
    }
}