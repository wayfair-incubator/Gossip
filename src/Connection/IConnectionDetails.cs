namespace Gossip.Connection
{
    /// <summary>
    /// Database connection details
    /// </summary>
    public interface IConnectionDetails
    {
        /// <summary>
        /// Server name (e.g. localhost)
        /// </summary>
        string Server { get; }

        /// <summary>
        /// Database name
        /// </summary>
        string Database { get; }
    }
}
