using AspNetUpgrade.UpgradeContext;

namespace AspNetUpgrade.Actions
{
    public interface ISolutionUpgradeAction
    {
        void Apply(ISolutionUpgradeContext solutionUpgradeContext);

        
    }
}