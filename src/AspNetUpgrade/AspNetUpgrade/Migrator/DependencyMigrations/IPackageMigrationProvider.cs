using System.Collections.Generic;
using AspNetUpgrade.Model;
using AspNetUpgrade.UpgradeContext;

namespace AspNetUpgrade.Migrator.DependencyMigrations
{
    public interface IDependencyPackageMigrationProvider
    {
        List<DependencyPackageMigrationInfo> GetPackageMigrations(ToolingVersion targetToolingVersion, IProjectUpgradeContext projectContext);

    }
}