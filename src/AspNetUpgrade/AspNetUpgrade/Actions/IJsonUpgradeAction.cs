using AspNetUpgrade.Upgrader;

namespace AspNetUpgrade.Actions
{
    public interface IJsonUpgradeAction
    {
        void Apply(IJsonProjectUpgradeContext fileUpgradeContext);

      //  void Undo(IJsonProjectUpgradeContext fileUpgradeContext);

    }
}