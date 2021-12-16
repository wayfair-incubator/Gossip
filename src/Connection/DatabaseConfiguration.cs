using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Gossip.Configuration;
using Gossip.Plugins;
using Gossip.Strategies;

namespace Gossip.Connection
{
    /// <summary>
    /// Database Configuration
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class DatabaseConfiguration
    {
        /// <summary>
        /// Timeout (in seconds) for sql command
        /// </summary>
        public int CommandTimeoutInSeconds { get; set; }

        /// <summary>
        /// Database execution strategy
        /// </summary>
        public IExecutionStrategy ExecutionStrategy { get; set; }

        /// <summary>
        /// List of database connections
        /// </summary>
        public List<Func<Task<IConnectionString>>> ConnectionStringFactories { get; set; }

        /// <summary>
        /// SQL connection factory
        /// </summary>
        public ISqlConnectionFactory ConnectionFactory { get; set; }

        /// <summary>
        /// Monitoring definition
        /// </summary>
        public IDatabaseMonitoring Monitoring { get; set; }

        internal IPluginManager PluginManager { get; set; }

        /// <summary>
        /// Database Configuration constructor
        /// </summary>
        /// <param name="connectionFactory">SQL connection factory</param>
        public DatabaseConfiguration(ISqlConnectionFactory connectionFactory)
        {
            Monitoring = new DatabaseMonitoring();
            CommandTimeoutInSeconds = 300;
            ConnectionStringFactories = new List<Func<Task<IConnectionString>>>();
            ExecutionStrategy = new NoopExecutionStrategy();
            PluginManager = new PluginManager();
            ConnectionFactory = connectionFactory;
        }
    }
}