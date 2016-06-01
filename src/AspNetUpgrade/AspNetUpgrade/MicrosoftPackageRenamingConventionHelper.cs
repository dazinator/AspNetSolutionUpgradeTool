using System.Collections.Generic;
using AspNetUpgrade.Actions;

namespace AspNetUpgrade
{
    public static class MicrosoftPackageRenamingConventionHelper
    {

        public enum ToolingVersion
        {
            Preview1,
        }

        public static List<NuGetPackageMigrationInfo> GetRc2NuGetPackagesList(ToolingVersion targetToolingVersion)
        {
            var list = new List<NuGetPackageMigrationInfo>();
            var package = new NuGetPackageMigrationInfo("Microsoft.EntityFrameworkCore.SqlServer", "1.0.0-rc2-final");
            package.OldNames.Add("EntityFramework.MicrosoftSqlServer");
            package.OldNames.Add("EntityFramework.SqlServer");
            list.Add(package);

            package = new NuGetPackageMigrationInfo("Microsoft.AspNetCore.Identity.EntityFrameworkCore", "1.0.0-rc2-final");
            package.OldNames.Add("Microsoft.AspNet.Identity.EntityFramework");
            list.Add(package);

            package = new NuGetPackageMigrationInfo("Microsoft.AspNetCore.Server.IISIntegration", "1.0.0-rc2-final");
            package.OldNames.Add("Microsoft.AspNet.IISPlatformHandler");
            list.Add(package);

            package = new NuGetPackageMigrationInfo("Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore", "1.0.0-rc2-final");
            package.OldNames.Add("Microsoft.AspNet.Diagnostics.Entity");
            list.Add(package);

            string toolingVersion = ToolingVersion.Preview1.ToString().ToLowerInvariant();

            package = new NuGetPackageMigrationInfo("Microsoft.AspNetCore.Razor.Tools", $"1.0.0-{toolingVersion}-final");
            package.Type = PackageType.Build;
            package.OldNames.Add("Microsoft.AspNet.Tooling.Razor");
            list.Add(package);

            package = new NuGetPackageMigrationInfo("Microsoft.EntityFrameworkCore.Tools", $"1.0.0-{toolingVersion}-final");
            package.Type = PackageType.Build;
            package.OldNames.Add("EntityFramework.Commands");
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


           // Microsoft.VisualStudio.Web.BrowserLink.Loader




            return list;

        }

      


    }

  
}