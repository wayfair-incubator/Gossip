using Gossip.ConnectionStrings;

namespace Gossip.TestSupport.Adapters.MockDb
{
    public class MockConnectionStringBuilder : IConnectionStringBuilder
    {
        public string Build(IConnectionStringSettings settings)
        {
            return string.Empty;
        }
    }
}