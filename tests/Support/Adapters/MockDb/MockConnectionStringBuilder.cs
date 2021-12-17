using System.Diagnostics.CodeAnalysis;
using Gossip.ConnectionStrings;

namespace Gossip.TestSupport.Adapters.MockDb
{
    [ExcludeFromCodeCoverage]
    public class MockConnectionStringBuilder : IConnectionStringBuilder
    {
        public string Build(IConnectionStringSettings settings)
        {
            return string.Empty;
        }
    }
}