using System.Collections.Generic;
using System.Linq;
using AspNetUpgrade.Actions;
using AspNetUpgrade.Actions.Csharp;
using AspNetUpgrade.UpgradeContext;

namespace AspNetUpgrade.Migrator
{
    public class CsharpCodeMigrator : BaseCsharpCodeMigrator
    {

        public CsharpCodeMigrationOptions Options { get; set; }


        public CsharpCodeMigrator(CsharpCodeMigrationOptions options) : base()
        {
            Options = options;
        }

        /// <summary>
        /// Migrates the csharp file using the current options.
        /// </summary>
        /// <param name="context">context representing he file to be upgraded..</param>
        /// <param name="additionalMigrations">any additional migrations to apply.</param>
        public override void Apply(BaseCsharpFileUpgradeContext context, IList<ICsharpCodeUpgradeAction> additionalMigrations)
        {
            List<ICsharpCodeUpgradeAction> migrations = new List<ICsharpCodeUpgradeAction>();
            //var context = this.Context;
            if (Options.RewriteUsings)
            {
                var rewriteUsings = new RewriteUsingStatements(new Rc2Rewriter());
                migrations.Add(rewriteUsings);
            }
            // additional migrations to apply
            if (additionalMigrations != null && additionalMigrations.Any())
            {
                migrations.AddRange(additionalMigrations);
            }
            base.Apply(context, migrations);
        }
    
    }
}