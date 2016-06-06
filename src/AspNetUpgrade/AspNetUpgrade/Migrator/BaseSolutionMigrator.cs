using System;
using System.Collections.Generic;
using AspNetUpgrade.Actions;
using AspNetUpgrade.UpgradeContext;

namespace AspNetUpgrade.Migrator
{
    public class BaseSolutionMigrator
    {

        public BaseSolutionMigrator(SolutionUpgradeContext context)
        {
            Context = context;
            AppliedActions = new List<ISolutionUpgradeAction>();
        }

        public void Apply(IList<ISolutionUpgradeAction> actions)
        {
            try
            {
                Context.BeginUpgrade(() =>
                {
                    foreach (var action in actions)
                    {
                        action.Apply(Context);
                        AppliedActions.Add(action);
                    }
                });
               
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public List<ISolutionUpgradeAction> AppliedActions { get; set; }

        public SolutionUpgradeContext Context { get; set; }


    }
}