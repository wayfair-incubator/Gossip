using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace Gossip.Configuration
{
    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public class DatabaseMonitoring : IDatabaseMonitoring
    {
        /// <summary>
        /// Initialize a new DatabaseMonitoring instance.
        /// </summary>
        public DatabaseMonitoring()
        {
            IsEnabled = false;
        }

        /// <inheritdoc />
        public CancellationToken CancellationToken { get; set; }
        /// <inheritdoc />
        public bool IsEnabled { get; set; }
        /// <inheritdoc />
        public TimeSpan Interval { get; set; }
    }
}