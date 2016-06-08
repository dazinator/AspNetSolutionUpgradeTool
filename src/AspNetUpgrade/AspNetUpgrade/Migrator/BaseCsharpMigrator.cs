using System;
using System.Collections.Generic;
using AspNetUpgrade.Actions;
using AspNetUpgrade.UpgradeContext;

namespace AspNetUpgrade.Migrator
{
    public class BaseCsharpCodeMigrator
    {
        public BaseCsharpCodeMigrator()
        {
            AppliedActions = new List<ICsharpCodeUpgradeAction>();
        }

        public virtual void Apply(BaseCsharpFileUpgradeContext context, IList<ICsharpCodeUpgradeAction> actions)
        {
            try
            {
                Context = context;
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

        public List<ICsharpCodeUpgradeAction> AppliedActions { get; set; }

        public BaseCsharpFileUpgradeContext Context { get; set; }


    }
}