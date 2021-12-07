using Gossip.Connection;
using Gossip.Connection.Fluent;
using Gossip.ConnectionStrings;

namespace Gossip.TestSupport.Adapters.MockDb
{
    public class MockDatabase : ISqlConnectionFactory
    {
        private readonly MockSqlConnection _conn;

        public MockDatabase()
        {
            _conn = new MockSqlConnection();
        }

        public ISqlConnection Create(IConnectionString connectionString)
        {
            return _conn;
        }

        public IConnectionStringBuilder GetConnectionStringBuilder()
        {
            return new MockConnectionStringBuilder();
        }

        public int GetTimeout()
        {
            return _conn.GetTimeout();
        }
    }
}