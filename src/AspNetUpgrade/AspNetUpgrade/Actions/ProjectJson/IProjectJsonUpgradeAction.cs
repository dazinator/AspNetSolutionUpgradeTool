using AspNetUpgrade.Upgrader;

namespace AspNetUpgrade.Actions.ProjectJson
{
    public interface IProjectJsonUpgradeAction
    {
        void Apply(IJsonFileUpgradeContext fileUpgradeContext);

        void Undo(IJsonFileUpgradeContext fileUpgradeContext);

    }
}