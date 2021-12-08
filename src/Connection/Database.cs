using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Gossip.Configuration;
using Gossip.Monitoring;
using Gossip.Plugins;
using Gossip.Strategies;

namespace Gossip.Connection
{
    /// <inheritdoc cref="IDatabaseConfigurator"/>
    public class Database : IDatabaseConfigurator
    {
        private readonly DatabaseConfiguration _config;

        /// <summary>
        /// Sets the database configuration
        /// </summary>
        /// <param name="connectionFactory">SQL Connection</param>
        private Database(ISqlConnectionFactory connectionFactory)
        {
            _config = new DatabaseConfiguration(connectionFactory);
        }

        /// <summary>
        /// Configures a database configuration
        /// </summary>
        /// <returns>IDatabaseConfigurator</returns>
        public static IDatabaseConfigurator Configure<T>() where T : ISqlConnectionFactory, new()
        {
            return Configure(new T());
        }

        /// <summary>
        /// Creates database configuration
        /// </summary>
        /// <param name="instance">SQL Connection</param>
        /// <returns>IDatabaseConfigurator</returns>
        public static IDatabaseConfigurator Configure<T>(T instance) where T : ISqlConnectionFactory
        {
            return new Database(instance);
        }

        /// <inheritdoc/>
        public IDatabaseConfigurator WithConnectionString(Func<IConnectionString> connectionString)
        {
            return WithConnectionString(() => Task.FromResult(connectionString()));
        }

        /// <inheritdoc/>
        public IDatabaseConfigurator WithConnectionString(Func<Task<IConnectionString>> connectionString)
        {
            _config.ConnectionStringFactories.Add(connectionString);
            return this;
        }

        /// <inheritdoc/>
        public IDatabaseConfigurator WithCommandTimeout(int timeoutInSeconds)
        {
            _config.CommandTimeoutInSeconds = timeoutInSeconds;
            return this;
        }

        /// <inheritdoc/>
        public IDatabaseConfigurator WithMonitoring(TimeSpan interval, CancellationToken cancellationToken)
        {
            _config.Monitoring = new DatabaseMonitoring
            {
                IsEnabled = true,
                CancellationToken = cancellationToken,
                Interval = interval
            };
            return this;
        }

        /// <inheritdoc />
        public IDatabaseConfigurator WithPlugin<T>(Func<T> pluginFactory) where T : IDatabasePlugin
        {
            _config.PluginManager.AddPlugin(pluginFactory);
            return this;
        }

        /// <inheritdoc/>
        public IDatabaseConfigurator WithExecutionStrategy(IExecutionStrategy executionStrategy)
        {
            _config.ExecutionStrategy = executionStrategy;
            return this;
        }

        /// <inheritdoc/>
        public IDatabaseConfigurator WithFallbacks(IEnumerable<Func<Task<IConnectionString>>> fallbacks)
        {
            _config.ConnectionStringFactories.AddRange(fallbacks);
            return this;
        }

        /// <inheritdoc/>
        public IDatabaseConnectionProvider Build()
        {
            var validator = new DatabaseConfigurationValidator();
            var validationResult = validator.Validate(_config);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.ToString());
            }

            if (_config.Monitoring.IsEnabled)
            {
                var monitor = new DatabaseConnectionMonitor(_config);
                Task.Run(() => monitor.RunAsync());
            }

            var databaseConnectionProvider = new DatabaseConnectionProvider(_config);

            var usage = new UsageDetails(Assembly.GetAssembly(typeof(Database)));
            foreach (var plugin in _config.PluginManager.InstantiatePlugins())
            {
                plugin.OnBuild(usage);
            }

            return databaseConnectionProvider;
        }
    }
}