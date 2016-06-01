using System.Collections.Generic;

namespace AspNetUpgrade.Actions
{
    public static class TargetNuGetPackages
    {

        public enum ToolingVersion
        {
            Preview1,
        }

        public static List<NuGetPackageInfo> GetRc2NuGetPackagesList(ToolingVersion targetToolingVersion)
        {
            var list = new List<NuGetPackageInfo>();
            var package = new NuGetPackageInfo("Microsoft.EntityFrameworkCore.SqlServer", "1.0.0-rc2-final");
            package.OldNames.Add("EntityFramework.MicrosoftSqlServer");
            package.OldNames.Add("EntityFramework.SqlServer");
            list.Add(package);

            package = new NuGetPackageInfo("Microsoft.AspNetCore.Identity.EntityFrameworkCore", "1.0.0-rc2-final");
            package.OldNames.Add("Microsoft.AspNet.Identity.EntityFramework");
            list.Add(package);

            package = new NuGetPackageInfo("Microsoft.AspNetCore.Server.IISIntegration", "1.0.0-rc2-final");
            package.OldNames.Add("Microsoft.AspNet.IISPlatformHandler");
            list.Add(package);

            package = new NuGetPackageInfo("Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore", "1.0.0-rc2-final");
            package.OldNames.Add("Microsoft.AspNet.Diagnostics.Entity");
            list.Add(package);

            string toolingVersion = ToolingVersion.Preview1.ToString().ToLowerInvariant();

            package = new NuGetPackageInfo("Microsoft.AspNetCore.Razor.Tools", $"1.0.0-{toolingVersion}-final");
            package.OldNames.Add("Microsoft.AspNet.Tooling.Razor");
            list.Add(package);

            package = new NuGetPackageInfo("Microsoft.EntityFrameworkCore.Tools", $"1.0.0-{toolingVersion}-final");
            package.OldNames.Add("EntityFramework.Commands");
            list.Add(package);

            package = new NuGetPackageInfo("Microsoft.VisualStudio.Web.CodeGenerators.Mvc", $"1.0.0-{toolingVersion}-final");
            package.OldNames.Add("Microsoft.Extensions.CodeGenerators.Mvc");
            list.Add(package);

            return list;

        }

    }
}