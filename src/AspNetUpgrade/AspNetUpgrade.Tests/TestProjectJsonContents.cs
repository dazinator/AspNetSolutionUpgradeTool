namespace AspNetUpgrade.Tests
{
    public static class TestProjectJsonContents
    {

        public const string Dnx451Project = @"{  
  'compilationOptions': {
    'emitEntryPoint': true
  },

  'dependencies': {   
  },
  'commands': {    
    'web': 'Microsoft.AspNet.Server.Kestrel',
    'ef': 'EntityFramework.Commands'
  },
  'frameworks': {
    'dnx451': {
      'dependencies': {
      },
      'frameworkAssemblies': {
        'System.Web': '4.0.0.0'
      }
    }
  }  
}";


        public const string DnxCore50Project = @"{  
  'compilationOptions': {
    'emitEntryPoint': true
  },

  'dependencies': {   
  },
  'commands': {    
    'web': 'Microsoft.AspNet.Server.Kestrel',
    'ef': 'EntityFramework.Commands'
  },
  'frameworks': {
    'dnxcore50': {
      'dependencies': {
        'System.Collections': '4.0.0.0',
        'System.Collections.Concurrent': '4.0.0.0',
        'System.ComponentModel': '4.0.0.0',
        'System.Linq': '4.0.0.0',
        'System.Reflection': '4.0.10.0',
        'System.Runtime': '4.0.20.0',
        'System.Runtime.InteropServices': '4.0.10.0',
        'System.Threading': '4.0.0.0',
        'System.Threading.Tasks': '4.0.0.0'
      }
    }
  }  
}";

        public const string WebApplicationProject = @"{
  'userSecretsId': 'aspnet5-WebApplication.Mvc-251553fe-641c-48a9-bcaf-e32f62d7861d',
  'version': '1.0.0-*',
  'webroot': 'wwwroot',
  'compilationOptions': {
    'emitEntryPoint': true
  },

  'dependencies': {
    'EntityFramework.Commands': '7.0.0-rc1-final',
    'EntityFramework.MicrosoftSqlServer': '7.0.0-rc1-final',
    'Microsoft.ApplicationInsights.AspNet': '1.0.0-rc1',
    'Microsoft.AspNet.Authentication.Cookies': '1.0.0-rc1-final',
    'Microsoft.AspNet.Diagnostics.Entity': '7.0.0-rc1-final',
    'Microsoft.AspNet.Identity.EntityFramework': '3.0.0-rc1-final',
    'Microsoft.AspNet.IISPlatformHandler': '1.0.0-rc1-final',
    'Microsoft.AspNet.Mvc': '6.0.0-rc1-final',
    'Microsoft.AspNet.Mvc.TagHelpers': '6.0.0-rc1-final',
    'Microsoft.AspNet.Server.Kestrel': '1.0.0-rc1-final',
    'Microsoft.AspNet.StaticFiles': '1.0.0-rc1-final',
    'Microsoft.AspNet.Tooling.Razor': '1.0.0-rc1-final',
    'Microsoft.Extensions.CodeGenerators.Mvc': '1.0.0-rc1-final',
    'Microsoft.Extensions.Configuration.FileProviderExtensions': '1.0.0-rc1-final',
    'Microsoft.Extensions.Configuration.Json': '1.0.0-rc1-final',
    'Microsoft.Extensions.Configuration.UserSecrets': '1.0.0-rc1-final',
    'Microsoft.Extensions.Logging': '1.0.0-rc1-final',
    'Microsoft.Extensions.Logging.Console': '1.0.0-rc1-final',
    'Microsoft.Extensions.Logging.Debug': '1.0.0-rc1-final',
    'Microsoft.VisualStudio.Web.BrowserLink.Loader': '14.0.0-rc1-final',
    'EntityFramework.MicrosoftSqlServer.Design': '7.0.0-rc1-final',
    'Autofac': '4.0.0-rc1-177',
    'Autofac.Extensions.DependencyInjection': '4.0.0-rc1-177',
    'Microsoft.Dnx.Runtime': '1.0.0-rc1-final'    
  },
  'commands': {    
    'web': 'Microsoft.AspNet.Server.Kestrel',
    'ef': 'EntityFramework.Commands'
  },

  'frameworks': {
    'dnx451': {
      'dependencies': {
      },
      'frameworkAssemblies': {
        'System.Web': '4.0.0.0'
      }
    }
  },

  'exclude': [
    'wwwroot',
    'node_modules',
    'shadowed',
    'artifacts',
    'modules'
  ],
  'publishExclude': [
    '**.user',
    '**.vspscc'
  ],
  'scripts': {
    'postrestore': [ 'npm install', 'bower install --config.interactive=false' ],
    'prepublish': [ 'npm install', 'bower install --config.interactive=false', 'gulp clean', 'gulp build' ]
  }
}
";

        public const string ConsoleProjectRc1 = @"{
  'version': '1.0.0-*',
  'description': 'ConsoleApp1 Console Application',
  'authors': [ 'daroth' ],
  'tags': [ '' ],
  'projectUrl': '',
  'licenseUrl': '',

  'compilationOptions': {
    'emitEntryPoint': true
  },

  'dependencies': {
  },

  'commands': {
    'ConsoleApp1': 'ConsoleApp1'
  },

  'frameworks': {
    'dnx451': { },
    'dnxcore50': {
      'dependencies': {
        'Microsoft.CSharp': '4.0.1-beta-23516',
        'System.Collections': '4.0.11-beta-23516',
        'System.Console': '4.0.0-beta-23516',
        'System.Linq': '4.0.1-beta-23516',
        'System.Threading': '4.0.11-beta-23516'
      }
    }
  }
}";

        public const string LibraryProjectRc1 = @"{
  'version': '1.0.0-*',
  'webroot': 'wwwroot',
  'description': 'Navigation Menu Module',
  'authors': [ 'Darrell.Tunnell' ],
  'tags': [ 'gluon-module' ],
  'projectUrl': '',
  'licenseUrl': '',
  'dependencies': {
    'Microsoft.AspNet.Mvc': '6.0.0-rc1-final',
    'Microsoft.AspNet.Mvc.TagHelpers': '6.0.0-rc1-final',
    'Gluon.Core': '1.0.0-*',
    'Gluon.Module.Amd': '1.0.0-*',
    'Gluon.Module.Bundler': '1.0.0-*',
    'Gluon.Module.Migrator': '1.0.0-*'
  },
  'frameworks': {
    'dnx451': { }
  },
  'packInclude': {
    'Content/': 'wwwroot/**/*.*'
  },
  'resource': 'Views/**',
  'scripts': {
    'prebuild': 'if not exist \'%project:Directory%\\..\\..\\artifacts\\bin\\Modules\' mkdir \'%project:Directory%\\..\\..\\artifacts\\bin\\Modules',
    'postpack': [ 'copy /y \'%project:Directory%\\..\\..\\artifacts\\bin\\%project:Name%\\%build:Configuration%\\%project:Name%.%project:Version%.nupkg\' \'%project:Directory%\\..\\..\\artifacts\\bin\\Modules\\%project:Name%.%project:Version%.nupkg' ]
  },
  'commands': {
    'ef': 'EntityFramework.Commands'
  }
}
";


    }







}
