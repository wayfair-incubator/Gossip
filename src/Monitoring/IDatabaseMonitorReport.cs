using System.Collections.Generic;
using Gossip.Connection;

namespace Gossip.Monitoring
{
    /// <summary>
    /// The monitor's report for the last interval
    /// </summary>
    public interface IDatabaseMonitorReport
    {
        /// <summary>
        /// A list of potential connections, in order of priority.
        /// </summary>
        IEnumerable<IConnectionDetails> ConnectionDetails { get; }
    }
}