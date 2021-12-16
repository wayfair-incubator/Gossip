using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Gossip.Connection;

namespace Gossip.Monitoring
{
    /// <inheritdoc/>
    [ExcludeFromCodeCoverage]
    internal class DatabaseMonitorReport : IDatabaseMonitorReport
    {
        /// <inheritdoc/>
        public IEnumerable<IConnectionDetails> ConnectionDetails { get; set; }
    }
}