using System;
using System.Collections.Generic;
using AspNetUpgrade.Actions;

namespace AspNetUpgrade.Upgrader
{
    public class ProjectMigrator
    {

        public ProjectMigrator(JsonProjectUpgradeContext context)
        {
            Context = context;
            AppliedActions = new List<IJsonUpgradeAction>();
        }

        public void Apply(IList<IJsonUpgradeAction> actions)
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

        public List<IJsonUpgradeAction> AppliedActions { get; set; }

        public JsonProjectUpgradeContext Context { get; set; }


    }


}