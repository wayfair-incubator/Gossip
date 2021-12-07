namespace Gossip.ConnectionStrings
{
    /// <summary>
    /// Builds the database connection string
    /// </summary>
    public interface IConnectionStringBuilder
    {
        /// <summary>
        /// Builds the database connection string
        /// </summary>
        /// <param name="settings">Database connection settings</param>
        /// <returns>Database connection string</returns>
        string Build(IConnectionStringSettings settings);
    }
}