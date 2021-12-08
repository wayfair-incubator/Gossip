using System.Collections.Generic;
using System.Threading.Tasks;
using Gossip.Connection;

namespace Gossip.Monitoring
{
    internal class DatabaseConnectionMonitor
    {
        private readonly DatabaseConfiguration _config;

        public DatabaseConnectionMonitor(DatabaseConfiguration config)
        {
            _config = config;
        }

        public async Task RunAsync()
        {
            while (!_config.Monitoring.CancellationToken.IsCancellationRequested)
            {
                _config.Monitoring.CancellationToken.ThrowIfCancellationRequested();

                var connectionStrings = new List<IConnectionDetails>();

                foreach (var factory in _config.ConnectionStringFactories)
                {
                    var connectionString = await factory();
                    connectionStrings.Add(connectionString);
                }

                foreach (var plugin in _config.PluginManager.InstantiatePlugins())
                {
                    await plugin.OnDatabaseMonitorExecutedAsync(new DatabaseMonitorReport
                    {
                        ConnectionDetails = connectionStrings
                    });
                }

                await Task.Delay(_config.Monitoring.Interval);
            }
        }
    }
}