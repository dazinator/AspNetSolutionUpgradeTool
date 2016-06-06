using AspNetUpgrade.Upgrader;

namespace AspNetUpgrade.Actions
{
    public interface ISolutionUpgradeAction
    {
        void Apply(ISolutionUpgradeContext solutionUpgradeContext);

        
    }
}