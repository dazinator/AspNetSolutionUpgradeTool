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

    }
}
