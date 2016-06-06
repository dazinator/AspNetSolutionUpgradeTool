using System;
using System.Collections.Generic;
using AspNetUpgrade.Actions;
using AspNetUpgrade.UpgradeContext;

namespace AspNetUpgrade.Migrator
{
    public class BaseProjectMigrator
    {

        public BaseProjectMigrator(BaseProjectUpgradeContext context)
        {
            Context = context;
            AppliedActions = new List<IProjectUpgradeAction>();
        }

        public void Apply(IList<IProjectUpgradeAction> actions)
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

        public List<IProjectUpgradeAction> AppliedActions { get; set; }

        public BaseProjectUpgradeContext Context { get; set; }


    }
}