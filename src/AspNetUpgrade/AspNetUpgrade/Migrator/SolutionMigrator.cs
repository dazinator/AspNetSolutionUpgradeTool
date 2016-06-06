using System.Collections.Generic;
using System.Linq;
using AspNetUpgrade.Actions;
using AspNetUpgrade.Actions.GlobalJson;
using AspNetUpgrade.UpgradeContext;

namespace AspNetUpgrade.Migrator
{
    public class SolutionMigrator : BaseSolutionMigrator
    {
        public SolutionMigrator(SolutionUpgradeContext context) : base(context)
        {
        }

        /// <summary>
        /// Migrates the solution.
        /// </summary>
        /// <param name="options">migration options.</param>
        /// <param name="additionalMigrations">any additional migrations to apply.</param>
        public void Apply(SolutionMigrationOptions options, IList<ISolutionUpgradeAction> additionalMigrations = null)
        {
            List<ISolutionUpgradeAction> migrations = new List<ISolutionUpgradeAction>();
            var context = this.Context;
            
            if (options.UpgradeToPreview1)
            {
                migrations.AddRange(GetSchemaUpgrades(options, context));
            }

            // additional migrations to apply
            if (additionalMigrations != null && additionalMigrations.Any())
            {
                migrations.AddRange(additionalMigrations);
            }

            this.Apply(migrations);
        }

        private IEnumerable<ISolutionUpgradeAction> GetSchemaUpgrades(SolutionMigrationOptions options, SolutionUpgradeContext context)
        {

            var upgradeActions = new List<ISolutionUpgradeAction>();

            // upgrades the compilation options section.
            var updateSdkVersion = new UpdateSdkVersion(options.SdkVersionNumber);
            upgradeActions.Add(updateSdkVersion);

            return upgradeActions;
        }
    }
}