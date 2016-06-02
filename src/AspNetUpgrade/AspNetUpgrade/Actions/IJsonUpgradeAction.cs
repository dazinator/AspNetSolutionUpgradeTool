using AspNetUpgrade.Upgrader;

namespace AspNetUpgrade.Actions
{
    public interface IJsonUpgradeAction
    {
        void Apply(IJsonFileUpgradeContext fileUpgradeContext);

        void Undo(IJsonFileUpgradeContext fileUpgradeContext);

    }
}