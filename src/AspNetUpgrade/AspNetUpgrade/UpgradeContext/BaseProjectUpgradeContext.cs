using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Build.Evaluation;
using Newtonsoft.Json.Linq;

namespace AspNetUpgrade.UpgradeContext
{
    public abstract class BaseProjectUpgradeContext : IProjectUpgradeContext
    {

        private JObject _projectJsonClone;
        private JObject _launchSettingsJsonClone;
        private StringBuilder _xprojBackup;

        public abstract JObject ProjectJsonObject { get; set; }

        public abstract JObject LaunchSettingsObject { get; set; }

        public abstract Project VsProjectFile { get; set; }

        public abstract void SaveChanges();

        public ProjectJsonWrapper ToProjectJsonWrapper()
        {
            return new ProjectJsonWrapper(ProjectJsonObject);
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
            _projectJsonClone = (JObject)ProjectJsonObject.DeepClone();
            _launchSettingsJsonClone = (JObject)LaunchSettingsObject.DeepClone();

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
            if (_projectJsonClone == null)
            {
                throw new InvalidOperationException("must call clone first");
            }
            ProjectJsonObject = _projectJsonClone;
            LaunchSettingsObject = _launchSettingsJsonClone;

            if (VsProjectFile != null)
            {
                VsProjectFile = VsProjectHelper.LoadTestProject(_xprojBackup.ToString());
            }

        }

        public List<BaseCsharpFileUpgradeContext> CsharpFiles { get; set; }
        public abstract string ProjectName();


    }


}