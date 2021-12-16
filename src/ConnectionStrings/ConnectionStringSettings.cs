using System.Diagnostics.CodeAnalysis;

namespace Gossip.ConnectionStrings
{
    /// <inheritdoc cref="IConnectionStringSettings"/>

    /// <summary>
    /// Database connection string settings
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ConnectionStringSettings : IConnectionStringSettings
    {
        /// <inheritdoc/>
        public string ApplicationName { get; set; }

        /// <inheritdoc/>
        public string MachineName { get; set; }

        /// <inheritdoc/>
        public string Server { get; set; }

        /// <inheritdoc/>
        public string Database { get; set; }

        /// <inheritdoc/>
        public int MaxPoolSize { get; set; }

        /// <inheritdoc/>
        public int DefaultCommandTimeout { get; set; }

        /// <inheritdoc/>
        public string Username { get; set; }

        /// <inheritdoc/>
        public string Password { get; set; }

        /// <inheritdoc/>
        public bool UseIntegratedSecurity { get; set; }
        
        /// <inheritdoc/>
        public int ConnectRetryCount { get; set; }

        /// <inheritdoc/>
        public int ConnectRetryInterval { get; set; }

        /// <inheritdoc/>
        public int LoadBalanceTimeout { get; set; }

        /// <inheritdoc/>
        public bool MultiSubnetFailover { get; set; }
    }
}