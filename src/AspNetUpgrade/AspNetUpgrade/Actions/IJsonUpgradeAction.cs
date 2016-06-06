using AspNetUpgrade.UpgradeContext;

namespace AspNetUpgrade.Actions
{
    public interface IProjectUpgradeAction
    {
        void Apply(IProjectUpgradeContext fileUpgradeContext);
    }
}