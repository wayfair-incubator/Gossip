using Gossip.Connection.Fluent;
using Gossip.ConnectionStrings;

namespace Gossip.Connection
{
    /// <summary>
    /// Creates connection to SQL databases
    /// </summary>
    public interface ISqlConnectionFactory
    {
        /// <summary>
        /// Create a database connection for a sql database
        /// </summary>
        /// <param name="connectionString">Connection string</param>
        /// <returns>Database connection</returns>
        ISqlConnection Create(IConnectionString connectionString);

        /// <summary>
        /// Gets a connection string builder
        /// </summary>
        /// <returns>Connection string builder</returns>
        IConnectionStringBuilder GetConnectionStringBuilder();
    }
}