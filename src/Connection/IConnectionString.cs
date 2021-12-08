using System.Data.SqlClient;

namespace Gossip.Connection
{
    /// <summary>
    /// Database connection string
    /// </summary>
    public interface IConnectionString : IConnectionDetails
    {
        /// <summary>
        /// Database connection string
        /// </summary>
        string Value { get; }

        /// <summary>
        /// Credentials for accessing the database
        /// </summary>
        SqlCredential Credentials { get; }
    }
}