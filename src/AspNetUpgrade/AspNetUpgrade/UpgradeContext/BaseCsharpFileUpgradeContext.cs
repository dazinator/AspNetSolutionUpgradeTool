using System;
using Microsoft.CodeAnalysis;

namespace AspNetUpgrade.UpgradeContext
{
    public abstract class BaseCsharpFileUpgradeContext : ICsharpFileUpgradeContext
    {
        private SyntaxTree _clone;

        public SyntaxTree Source { get; set; }

        public abstract void SaveChanges();

        public void BeginUpgrade(Action callback)
        {
            try
            {
                Clone();
                callback();
            }
            catch (Exception e)
            {
                // if there is an error during the upgrade, restore the file to snapshot.
                RestoreClone();
                throw;
            }
        }

        private void Clone()
        {
            _clone = Source;
        }

        private void RestoreClone()
        {
            if (_clone == null)
            {
                throw new InvalidOperationException("must call clone first");
            }
            Source = _clone;
        }

        protected bool HasChanged()
        {
            bool hasChanged = Source.GetRoot() != _clone.GetRoot();
            return hasChanged;
        }

    }
}