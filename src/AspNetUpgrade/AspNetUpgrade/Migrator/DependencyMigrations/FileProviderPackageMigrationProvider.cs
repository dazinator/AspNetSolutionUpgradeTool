using System.Collections.Generic;
using AspNetUpgrade.Model;
using AspNetUpgrade.UpgradeContext;

namespace AspNetUpgrade.Migrator.DependencyMigrations
{
    public class FileProviderDependencyPackageMigrationProvider : IDependencyPackageMigrationProvider
    {
        public List<DependencyPackageMigrationInfo> GetPackageMigrations(ToolingVersion targetToolingVersion, IProjectUpgradeContext projectContext)
        {
            var list = new List<DependencyPackageMigrationInfo>();
            string toolingVersion = ToolingVersion.Preview1.ToString().ToLowerInvariant();


            // file providers were renamed.
            var package = new DependencyPackageMigrationInfo("Microsoft.Extensions.FileProviders.Abstractions", "1.0.0-rc2-final");
            package.OldNames.Add("Microsoft.AspNet.FileProviders");
            package.MigrationAction = PackageMigrationAction.Update;
            list.Add(package);

            package = new DependencyPackageMigrationInfo("Microsoft.Extensions.FileProviders.Composite", "1.0.0-rc2-final");
            package.OldNames.Add("Microsoft.AspNet.FileProviders.Composite");
            package.MigrationAction = PackageMigrationAction.Update;
            list.Add(package);

            package = new DependencyPackageMigrationInfo("Microsoft.Extensions.FileProviders.Embedded", "1.0.0-rc2-final");
            package.OldNames.Add("Microsoft.AspNet.FileProviders.Embedded");
            package.MigrationAction = PackageMigrationAction.Update;
            list.Add(package);

            package = new DependencyPackageMigrationInfo("Microsoft.Extensions.FileProviders.Physical", "1.0.0-rc2-final");
            package.OldNames.Add("Microsoft.AspNet.FileProviders.Physical");
            package.MigrationAction = PackageMigrationAction.Update;
            list.Add(package);


            return list;
        }
    }
}