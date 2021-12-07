using Gossip.ConnectionStrings;

namespace Gossip.TestSupport.Adapters.Sqlite
{
    public class SqliteConnectionStringBuilder : IConnectionStringBuilder
    {
        public string Build(IConnectionStringSettings settings)
        {
            return settings.Server;
        }
    }
}