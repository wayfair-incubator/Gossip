using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Gossip.Connection.Exceptions;
using Gossip.Connection.Fluent;

namespace Gossip.Connection
{
    /// <inheritdoc cref="IDatabaseConnectionProvider"/>
    public class DatabaseConnectionProvider : IDatabaseConnectionProvider
    {
        private readonly DatabaseConfiguration _config;

        /// <summary>
        /// Gets a database connection
        /// </summary>
        /// <param name="config">Configuration for the database connection</param>
        internal DatabaseConnectionProvider(DatabaseConfiguration config)
        {
            _config = config;
        }

        /// <inheritdoc/>
        public Task<IDatabaseConnection> OpenAsync()
        {
            return OpenAsync(new CancellationTokenSource().Token);
        }

        /// <inheritdoc/>
        public async Task<IDatabaseConnection> OpenAsync(CancellationToken cancellationToken)
        {
            var sqlConnection = await CreateAndOpenConnectionAsync(cancellationToken);
            var queryExecutor = new QueryExecutorProvider(_config.PluginManager, _config.ExecutionStrategy);
            return new DatabaseConnection(sqlConnection, _config.CommandTimeoutInSeconds, queryExecutor, cancellationToken);
        }

        /// <summary>
        /// Creates and opens a database connection
        /// </summary>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns></returns>
        private async Task<ISqlConnection> CreateAndOpenConnectionAsync(CancellationToken cancellationToken)
        {
            Exception lastException = null;
            var logs = new StringBuilder();

            var queue = new Queue<Func<Task<IConnectionString>>>();
            foreach (var factory in _config.ConnectionStringFactories)
            {
                queue.Enqueue(factory);
            }

            var plugins = _config.PluginManager.InstantiatePlugins();

            while (queue.Count > 0)
            {
                var connectionStringFactory = queue.Dequeue();
                IConnectionString connectionString = null;

                try
                {
                    connectionString = await connectionStringFactory().ConfigureAwait(false);
                    var sqlConnection = _config.ConnectionFactory.Create(connectionString);

                    foreach (var plugin in plugins)
                    {
                        await plugin.OnConnectionOpeningAsync(connectionString);
                    }

                    var stopwatch = Stopwatch.StartNew();

                    await sqlConnection.OpenAsync(cancellationToken);

                    stopwatch.Stop();
                    var executionDetails = new ExecutionDetails(stopwatch.Elapsed);

                    foreach (var plugin in plugins)
                    {
                        await plugin.OnConnectionOpenAsync(connectionString, executionDetails);
                    }

                    return sqlConnection;
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch (DatabaseResolutionException ex)
                {
                    logs.AppendLine(GetDatabaseResolutionExceptionLogLine(ex));
                    lastException = ex;

                    foreach (var plugin in plugins)
                    {
                        await plugin.OnDatabaseResolutionExceptionAsync(ex.Database);
                    }
                }
                catch (Exception ex)
                {
                    logs.AppendLine(GetExceptionLogLine(connectionString, ex));
                    lastException = ex;

                    foreach (var plugin in plugins)
                    {
                        await plugin.OnConnectionExceptionAsync(connectionString);
                    }
                }
            }

            throw new DatabaseConnectionException($"Unable to connect to any databases. Logs: {logs}", lastException);
        }

        private string GetExceptionLogLine(IConnectionString connectionString, Exception ex)
        {
            if (connectionString == null)
            {
                return $"Unable to find a connection string to connect to. Failed due to {ex.GetType().Name} thrown ({ex.Message}).";
            }

            return $"Attempted to connect to {connectionString.Server}.{connectionString.Database} but failed due to {ex.GetType().Name} thrown ({ex.Message}).";
        }

        private static string GetDatabaseResolutionExceptionLogLine(DatabaseResolutionException databaseResolutionException)
        {
            return $"Attempted to connect to {databaseResolutionException.Database} but failed due to DatabaseResolutionException thrown ({databaseResolutionException.Message}).";
        }
    }
}
