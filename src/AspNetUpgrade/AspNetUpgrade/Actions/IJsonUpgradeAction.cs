using AspNetUpgrade.Upgrader;

namespace AspNetUpgrade.Actions
{
    public interface IProjectUpgradeAction
    {
        void Apply(IJsonProjectUpgradeContext fileUpgradeContext);

      //  void Undo(IJsonProjectUpgradeContext fileUpgradeContext);

    }
}