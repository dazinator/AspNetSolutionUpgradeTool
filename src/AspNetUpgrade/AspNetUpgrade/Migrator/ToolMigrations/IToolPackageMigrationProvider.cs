using System.Collections.Generic;
using AspNetUpgrade.Model;
using AspNetUpgrade.UpgradeContext;

namespace AspNetUpgrade.Migrator.ToolMigrations
{
    public interface IToolPackageMigrationProvider
    {
        List<ToolPackageMigrationInfo> GetPackageMigrations(ToolingVersion targetToolingVersion, IProjectUpgradeContext projectContext);

    }
}