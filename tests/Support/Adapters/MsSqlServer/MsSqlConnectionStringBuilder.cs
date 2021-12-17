using System.Diagnostics.CodeAnalysis;
using Gossip.ConnectionStrings;

namespace Gossip.TestSupport.Adapters.MsSqlServer
{
    [ExcludeFromCodeCoverage]
    public class MsSqlConnectionStringBuilder : IConnectionStringBuilder
    {
        public string Build(IConnectionStringSettings settings)
        {
            return settings.Server;
        }
    }
}