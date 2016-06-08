using System;
using Microsoft.CodeAnalysis;

namespace AspNetUpgrade.UpgradeContext
{
    public interface ICsharpFileUpgradeContext
    {
        SyntaxTree Source { get; set; }

        void SaveChanges();

        void BeginUpgrade(Action callback);

       // void Clone();

       // void RestoreClone();
    }
}