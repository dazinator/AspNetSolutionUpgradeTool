using System;
using System.Collections.Generic;
using AspNetUpgrade.Actions;

namespace AspNetUpgrade.Upgrader
{
    public class ProjectMigrator
    {

        private JsonProjectUpgradeContext _context;

        public ProjectMigrator(JsonProjectUpgradeContext context)
        {
            _context = context;
            AppliedActions = new List<IJsonUpgradeAction>();
        }

        public void Apply(IList<IJsonUpgradeAction> actions)
        {
            try
            {
                _context.BeginUpgrade(() =>
                {
                    foreach (var action in actions)
                    {
                        action.Apply(_context);
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

    }
}