using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace AspNetUpgrade.UpgradeContext
{
    public class CsharpFileUpgradeContext : BaseCsharpFileUpgradeContext
    {

        private FileInfo _csharpFileInfo;

        public CsharpFileUpgradeContext(FileInfo csharpFileInfo)
        {
            _csharpFileInfo = csharpFileInfo;
            var text = File.ReadAllText(csharpFileInfo.FullName);
            SyntaxTree tree = CSharpSyntaxTree.ParseText(text);
            Source = tree;
        }

        public override void SaveChanges()
        {
            if (HasChanged())
            {
                using (var writer = new StreamWriter(_csharpFileInfo.FullName))
                {
                    writer.Write(Source.ToString());
                    writer.Flush();
                }
            }
           
        }

        public override string ToString()
        {
            return _csharpFileInfo.FullName;
        }


    }
}