using System.Collections.Generic;
using AspNetUpgrade.Model;
using AspNetUpgrade.UpgradeContext;

namespace AspNetUpgrade.Migrator.DependencyMigrations.RTM
{
    public class AutofacRtmPackageMigrationProvider : IDependencyPackageMigrationProvider
    {
        public List<DependencyPackageMigrationInfo> GetPackageMigrations(ToolingVersion targetToolingVersion, IProjectUpgradeContext projectContext)
        {
            var list = new List<DependencyPackageMigrationInfo>();
            string toolingVersion = ToolingVersion.Preview1.ToString().ToLowerInvariant();

            // aspnet packages
            var package = new DependencyPackageMigrationInfo("Autofac", "4.0.0-rc3-280");
            list.Add(package);

            package = new DependencyPackageMigrationInfo("Autofac.Extensions.DependencyInjection", "4.0.0-rc3-280");
            list.Add(package);


            return list;
        }
    }


}