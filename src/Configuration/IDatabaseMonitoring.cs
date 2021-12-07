using System;
using System.Threading;

namespace Gossip.Configuration
{
    /// <summary>
    /// Configuration for monitoring the databases.
    /// </summary>
    public interface IDatabaseMonitoring
    {
        /// <summary>
        /// Cancellation token
        /// </summary>
        CancellationToken CancellationToken { get; }

        /// <summary>
        /// Whether the monitoring is active
        /// </summary>
        bool IsEnabled { get; }

        /// <summary>
        /// How frequently to run the monitor
        /// </summary>
        TimeSpan Interval { get; }
    }
}