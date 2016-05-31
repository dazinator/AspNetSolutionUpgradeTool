namespace AspNetUpgrade.Actions
{
    public interface IAction
    {
        void Apply(IJsonFileUpgradeContext fileUpgradeContext);

        void Undo(IJsonFileUpgradeContext fileUpgradeContext);

    }
}