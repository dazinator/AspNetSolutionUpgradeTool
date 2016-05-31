﻿namespace AspNetUpgrade.Tests
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

    }
}