using Gossip.Connection.Fluent;
using Gossip.ConnectionStrings;

namespace Gossip.Connection.Factories
{
    /// <summary>
    ///  Creates connection to MsSQL databases
    /// </summary>
    public class MsSql : ISqlConnectionFactory
    {
        /// <inheritdoc/>
        public ISqlConnection Create(IConnectionString connectionString)
        {
            return new SqlConnectionWrapper(connectionString);
        }

        /// <inheritdoc/>
        public IConnectionStringBuilder GetConnectionStringBuilder()
        {
            return new MsSqlConnectionStringBuilder();
        }
    }
}