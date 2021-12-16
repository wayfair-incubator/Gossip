using System.Collections.Generic;
using Gossip.Connection;

namespace Gossip.Monitoring
{
    /// <inheritdoc/>
    internal class DatabaseMonitorReport : IDatabaseMonitorReport
    {
        /// <inheritdoc/>
        public IEnumerable<IConnectionDetails> ConnectionDetails { get; set; }
    }
}