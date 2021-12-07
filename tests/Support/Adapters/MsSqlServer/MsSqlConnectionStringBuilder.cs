using Gossip.ConnectionStrings;

namespace Gossip.TestSupport.Adapters.MsSqlServer
{
    public class MsSqlConnectionStringBuilder : IConnectionStringBuilder
    {
        public string Build(IConnectionStringSettings settings)
        {
            return settings.Server;
        }
    }
}