using AspNetUpgrade.UpgradeContext;

namespace AspNetUpgrade.Actions
{
    public interface ICsharpCodeUpgradeAction
    {
        void Apply(ICsharpFileUpgradeContext upgradeContext);

    }
}