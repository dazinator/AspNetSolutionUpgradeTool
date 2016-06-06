using System;
using System.Collections.Generic;
using AspNetUpgrade.Actions;

namespace AspNetUpgrade.Upgrader
{
    public class BaseProjectMigrator
    {

        public BaseProjectMigrator(JsonProjectUpgradeContext context)
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

        public JsonProjectUpgradeContext Context { get; set; }


    }


}