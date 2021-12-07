using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Gossip.Plugins;
using Gossip.Strategies;

namespace Gossip.Connection
{
    /// <summary>
    /// Configures a database connection
    /// </summary>
    public interface IDatabaseConfigurator
    {
        /// <summary>
        /// Adds a connection string to the database connection
        /// </summary>
        /// <param name="connectionString">Database connection string</param>
        /// <returns>IDatabaseConfigurator</returns>
        IDatabaseConfigurator WithConnectionString(Func<IConnectionString> connectionString);

        /// <summary>
        /// Builds a connection string to configure a database connection
        /// </summary>
        /// <param name="connectionString">Database Connection String</param>
        /// <returns>IDatabaseConfigurator</returns>
        IDatabaseConfigurator WithConnectionString(Func<Task<IConnectionString>> connectionString);

        /// <summary>
        /// Time in ms that the connection will wait for a command to execute
        /// </summary>
        /// <param name="timeoutInSeconds">Timeout (in seconds)</param>
        /// <returns>IDatabaseConfigurator</returns>
        IDatabaseConfigurator WithCommandTimeout(int timeoutInSeconds);

        /// <summary>
        /// Add a plugin by providing a factory that will be used to instantiate the plugin every call to the database.
        /// </summary>
        /// <param name="pluginFactory">The plugin to add.</param>
        /// <returns>IDatabaseConfigurator</returns>
        IDatabaseConfigurator WithPlugin<T>(Func<T> pluginFactory) where T : IDatabasePlugin;

        /// <summary>
        /// Adds an execution strategy to the database connection
        /// </summary>
        /// <param name="executionStrategy">Execution strategy</param>
        /// <returns>IDatabaseConfigurator</returns>
        IDatabaseConfigurator WithExecutionStrategy(IExecutionStrategy executionStrategy);

        /// <summary>
        /// Adds fallbacks to the database connection
        /// </summary>
        /// <param name="fallbacks">Connection fallbacks</param>
        /// <returns>IDatabaseConfigurator</returns>
        IDatabaseConfigurator WithFallbacks(IEnumerable<Func<Task<IConnectionString>>> fallbacks);

        /// <summary>
        /// Get insight into the state of the library. See what databases are in line to be used.
        /// </summary>
        /// <param name="interval">The frequency for the monitor to run</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        IDatabaseConfigurator WithMonitoring(TimeSpan interval, CancellationToken cancellationToken = default);

        /// <summary>
        /// Builds the database connection
        /// </summary>
        /// <returns>IDatabaseConfigurator</returns>
        IDatabaseConnectionProvider Build();
    }
}