using System.Data.SqlClient;

namespace Gossip.Connection
{
    /// <inheritdoc cref="IConnectionString"/>
    public class ConnectionString : IConnectionString
    {
        public string Value { get; set; }

        public string Server { get; set; }

        public string Database { get; set; }

        public SqlCredential Credentials { get; set; }
    }
}