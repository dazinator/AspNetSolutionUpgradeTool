namespace AspNetUpgrade.Model
{
    public enum PackageMigrationAction
    {
        /// <summary>
        /// Adds the package if its not present, or updates it if it is already found.
        /// </summary>
        AddOrUpdate,
        /// <summary>
        /// Updates the package if it is present, else does nothing.
        /// </summary>
        Update,
        /// <summary>
        /// Removes the package if it is present.
        /// </summary>
        Remove
    }
}