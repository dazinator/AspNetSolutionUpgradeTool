using System.Collections.Generic;
using AspNetUpgrade.Model;
using AspNetUpgrade.UpgradeContext;

namespace AspNetUpgrade.Migrator.DependencyMigrations.RTM
{
    public class EntityFrameworkRtmDependencyPackageMigrationProvider : IDependencyPackageMigrationProvider
    {

        public List<DependencyPackageMigrationInfo> GetPackageMigrations(ToolingVersion targetToolingVersion, IProjectUpgradeContext projectContext)
        {
            var list = new List<DependencyPackageMigrationInfo>();
            string toolingVersion = targetToolingVersion.ToString().ToLowerInvariant();


            // ef packages..
            var package = new DependencyPackageMigrationInfo("Microsoft.EntityFrameworkCore.SqlServer", "1.0.0");
            package.OldNames.Add("EntityFramework.MicrosoftSqlServer");
            package.OldNames.Add("EntityFramework.SqlServer");
            list.Add(package);

            package = new DependencyPackageMigrationInfo("Microsoft.EntityFrameworkCore.SQLite", "1.0.0");
            package.OldNames.Add("EntityFramework.SQLite");
            list.Add(package);

            //package = new NuGetPackageMigrationInfo("NpgSql.EntityFrameworkCore.Postgres", "1.0.0-rc2-final");
            //package.OldNames.Add("EntityFramework7.Npgsql");
            //list.Add(package);

            package = new DependencyPackageMigrationInfo("EntityFrameworkCore.SqlServerCompact35", "1.0.0");
            package.OldNames.Add("EntityFramework.SqlServerCompact35");
            list.Add(package);

            package = new DependencyPackageMigrationInfo("EntityFrameworkCore.SqlServerCompact40", "1.0.0");
            package.OldNames.Add("EntityFramework.SqlServerCompact40");
            list.Add(package);

            package = new DependencyPackageMigrationInfo("Microsoft.EntityFrameworkCore.InMemory", "1.0.0");
            package.OldNames.Add("EntityFramework.InMemory");
            list.Add(package);

            //package = new NuGetPackageMigrationInfo("EntityFramework.IBMDataServer", "1.0.0-rc2-final");
            //package.OldNames.Add("EntityFramework.IBMDataServer");
            //list.Add(package);

            package = new DependencyPackageMigrationInfo("Microsoft.EntityFrameworkCore.Tools", $"1.0.0-{toolingVersion}-final");
            package.Type = PackageType.Build;
            package.OldNames.Add("EntityFramework.Commands");
            list.Add(package);


            package = new DependencyPackageMigrationInfo("Microsoft.EntityFrameworkCore.SqlServer.Design", "1.0.0");
            package.OldNames.Add("EntityFramework.MicrosoftSqlServer.Design");
            list.Add(package);



            return list;
        }
    }
}