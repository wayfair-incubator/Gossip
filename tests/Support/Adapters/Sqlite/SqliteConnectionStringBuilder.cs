using System.Diagnostics.CodeAnalysis;
using Gossip.ConnectionStrings;

namespace Gossip.TestSupport.Adapters.Sqlite
{
    [ExcludeFromCodeCoverage]
    public class SqliteConnectionStringBuilder : IConnectionStringBuilder
    {
        public string Build(IConnectionStringSettings settings)
        {
            return settings.Server;
        }
    }
}