using AspNetUpgrade.Upgrader;

namespace AspNetUpgrade.Actions.Xproj
{
    public class MigrateProjectImportsFromDnxToDotNet : IJsonUpgradeAction
    {
        public void Apply(IJsonProjectUpgradeContext fileUpgradeContext)
        {
            if (fileUpgradeContext.VsProjectFile != null)
            {
                bool isWebProject = fileUpgradeContext.ToProjectJsonWrapper().IsMvcProject();
                InstallTargetsHelper.UpgradeDnxProjectToDotNet(fileUpgradeContext.VsProjectFile, isWebProject);
            }
        }
    }
}
