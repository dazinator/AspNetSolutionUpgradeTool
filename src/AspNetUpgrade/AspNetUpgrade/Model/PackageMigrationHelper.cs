using System.Collections.Generic;

namespace AspNetUpgrade.Model
{
    public static class PackageMigrationHelper
    {

        public enum ToolingVersion
        {
            Preview1,
        }

        public static List<NuGetPackageMigrationInfo> GetRc2PackageMigrationList(ToolingVersion targetToolingVersion)
        {
            var list = new List<NuGetPackageMigrationInfo>();
            string toolingVersion = ToolingVersion.Preview1.ToString().ToLowerInvariant();

            // ef packages..
            var package = new NuGetPackageMigrationInfo("Microsoft.EntityFrameworkCore.SqlServer", "1.0.0-rc2-final");
            package.OldNames.Add("EntityFramework.MicrosoftSqlServer");
            package.OldNames.Add("EntityFramework.SqlServer");
            list.Add(package);

            package = new NuGetPackageMigrationInfo("Microsoft.EntityFrameworkCore.SQLite", "1.0.0-rc2-final");
            package.OldNames.Add("EntityFramework.SQLite");
            list.Add(package);

            //package = new NuGetPackageMigrationInfo("NpgSql.EntityFrameworkCore.Postgres", "1.0.0-rc2-final");
            //package.OldNames.Add("EntityFramework7.Npgsql");
            //list.Add(package);

            package = new NuGetPackageMigrationInfo("EntityFrameworkCore.SqlServerCompact35", "1.0.0-rc2-final");
            package.OldNames.Add("EntityFramework.SqlServerCompact35");
            list.Add(package);
            
            package = new NuGetPackageMigrationInfo("EntityFrameworkCore.SqlServerCompact40", "1.0.0-rc2-final");
            package.OldNames.Add("EntityFramework.SqlServerCompact40");
            list.Add(package);

            package = new NuGetPackageMigrationInfo("Microsoft.EntityFrameworkCore.InMemory", "1.0.0-rc2-final");
            package.OldNames.Add("EntityFramework.InMemory");
            list.Add(package);


            //package = new NuGetPackageMigrationInfo("EntityFramework.IBMDataServer", "1.0.0-rc2-final");
            //package.OldNames.Add("EntityFramework.IBMDataServer");
            //list.Add(package);

            package = new NuGetPackageMigrationInfo("Microsoft.EntityFrameworkCore.Tools", $"1.0.0-{toolingVersion}-final");
            package.Type = PackageType.Build;
            package.OldNames.Add("EntityFramework.Commands");
            list.Add(package);


            package = new NuGetPackageMigrationInfo("Microsoft.EntityFrameworkCore.SqlServer.Design", "1.0.0-rc2-final");
            package.OldNames.Add("EntityFramework.MicrosoftSqlServer.Design");
            list.Add(package);


            // aspnet packages
            package = new NuGetPackageMigrationInfo("Microsoft.AspNetCore.Identity.EntityFrameworkCore", "1.0.0-rc2-final");
            package.OldNames.Add("Microsoft.AspNet.Identity.EntityFramework");
            list.Add(package);

            package = new NuGetPackageMigrationInfo("Microsoft.AspNetCore.Server.IISIntegration", "1.0.0-rc2-final");
            package.OldNames.Add("Microsoft.AspNet.IISPlatformHandler");
            list.Add(package);

            package = new NuGetPackageMigrationInfo("Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore", "1.0.0-rc2-final");
            package.OldNames.Add("Microsoft.AspNet.Diagnostics.Entity");
            list.Add(package);
            

            package = new NuGetPackageMigrationInfo("Microsoft.AspNetCore.Razor.Tools", $"1.0.0-{toolingVersion}-final");
            package.Type = PackageType.Build;
            package.OldNames.Add("Microsoft.AspNet.Tooling.Razor");
            list.Add(package);
        

            package = new NuGetPackageMigrationInfo("Microsoft.VisualStudio.Web.CodeGenerators.Mvc", $"1.0.0-{toolingVersion}-final");
            package.OldNames.Add("Microsoft.Extensions.CodeGenerators.Mvc");
            package.Type = PackageType.Build;
            list.Add(package);

            package = new NuGetPackageMigrationInfo("Microsoft.ApplicationInsights.AspNetCore", "1.0.0-rc2-final");
            package.OldNames.Add("Microsoft.ApplicationInsights.AspNet");
            list.Add(package);

            package = new NuGetPackageMigrationInfo("Microsoft.Extensions.Configuration.FileExtensions", "1.0.0-rc2-final");
            package.OldNames.Add("Microsoft.Extensions.Configuration.FileProviderExtensions");
            list.Add(package);

            // remove Microsoft.Dnx.Runtime
            package = new NuGetPackageMigrationInfo("Microsoft.Dnx.Runtime", "1.0.0-rc1-final");
            package.MigrationAction = PackageMigrationAction.Remove;
            list.Add(package);

            // Microsoft.VisualStudio.Web.BrowserLink.Loader




            return list;

        }




    }


}