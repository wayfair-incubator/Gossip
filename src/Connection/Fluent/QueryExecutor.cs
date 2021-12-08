using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Gossip.Plugins;
using Gossip.Strategies;

namespace Gossip.Connection.Fluent
{
    /// <inheritdoc cref="IQueryExecutor"/>
    public class QueryExecutor : IUpdatableQueryExecutor, IBulkQueryExecutor
    {
        private readonly ISqlConnection _conn;
        private QueryConfiguration _config;
        private readonly FunctionMetadata _metadata;
        private readonly IPluginManager _pluginManager;
        private readonly IExecutionStrategy _executionStrategy;

        public QueryExecutor(
            ISqlConnection conn,
            QueryConfiguration config,
            FunctionMetadata metadata,
            IPluginManager pluginManager,
            IExecutionStrategy executionStrategy)
        {
            _conn = conn;
            _config = config;
            _metadata = metadata;
            _pluginManager = pluginManager;
            _executionStrategy = executionStrategy;
        }

        public T QueryFirstOrDefault<T>()
        {
            return PerformQuery(c => c.QueryFirstOrDefault<T>(_config.Query, _config.Parameters, _config.Timeout, _config.CancellationToken, _config.Transaction.Value));
        }

        public Task<T> QueryFirstOrDefaultAsync<T>()
        {
            return PerformQueryAsync(c => c.QueryFirstOrDefaultAsync<T>(_config.Query, _config.Parameters, _config.Timeout, _config.CancellationToken, _config.Transaction.Value));
        }

        public T QuerySingleOrDefault<T>()
        {
            return PerformQuery(c => c.QuerySingleOrDefault<T>(_config.Query, _config.Parameters, _config.Timeout, _config.CancellationToken, _config.Transaction.Value));
        }

        public Task<T> QuerySingleOrDefaultAsync<T>()
        {
            return PerformQueryAsync(c => c.QuerySingleOrDefaultAsync<T>(_config.Query, _config.Parameters, _config.Timeout, _config.CancellationToken, _config.Transaction.Value));
        }

        public IEnumerable<T> Query<T>()
        {
            return PerformQuery(c => c.Query<T>(_config.Query, _config.Parameters, _config.Timeout, !_config.Unbuffered, _config.Transaction.Value));
        }

        public Task<IEnumerable<T>> QueryAsync<T>()
        {
            return PerformQueryAsync(c => c.QueryAsync<T>(_config.Query, _config.Parameters, _config.Timeout, _config.CancellationToken, _config.Transaction.Value));
        }

        public Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TReturn>(Func<TFirst, TSecond, TReturn> mapping)
        {
            return PerformQueryAsync(c => c.QueryAsync(_config.Query, mapping, _config.Parameters, _config.Timeout, _config.Transaction.Value));
        }

        public Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TReturn>(Func<TFirst, TSecond, TThird, TReturn> mapping)
        {
            return PerformQueryAsync(c => c.QueryAsync(_config.Query, mapping, _config.Parameters, _config.Timeout, _config.Transaction.Value));
        }

        public Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TReturn>(Func<TFirst, TSecond, TThird, TFourth, TReturn> mapping)
        {
            return PerformQueryAsync(c => c.QueryAsync(_config.Query, mapping, _config.Parameters, _config.Timeout, _config.Transaction.Value));
        }

        public Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> mapping)
        {
            return PerformQueryAsync(c => c.QueryAsync(_config.Query, mapping, _config.Parameters, _config.Timeout, _config.Transaction.Value));
        }

        public Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> mapping)
        {
            return PerformQueryAsync(c => c.QueryAsync(_config.Query, mapping, _config.Parameters, _config.Timeout, _config.Transaction.Value));
        }

        public Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> mapping)
        {
            return PerformQueryAsync(c => c.QueryAsync(_config.Query, mapping, _config.Parameters, _config.Timeout, _config.Transaction.Value));
        }

        public Task<int> ExecuteAsync()
        {
            return PerformQueryAsync(c => c.ExecuteAsync(_config.Query, _config.Parameters, _config.Timeout, _config.CancellationToken, _config.Transaction.Value));
        }

        public T ExecuteScalar<T>()
        {
            return PerformQuery(c => c.ExecuteScalar<T>(_config.Query, _config.Parameters, _config.Timeout, _config.Transaction.Value));
        }

        public Task<T> ExecuteScalarAsync<T>()
        {
            return PerformQueryAsync(c => c.ExecuteScalarAsync<T>(_config.Query, _config.Parameters, _config.Timeout, _config.CancellationToken, _config.Transaction.Value));
        }

        public int Execute()
        {
            return PerformQuery(c => c.Execute(_config.Query, _config.Parameters, _config.Timeout, _config.CancellationToken, _config.Transaction.Value));
        }

        public IPartitionConfigurator<T> BatchedBy<T>(IEnumerable<T> partitionedBy)
        {
            return new PartitionedQueryConfigurator<T>(this, _config, partitionedBy);
        }

        public Task InsertInBulkAsync<T>(IEnumerable<T> data, string tableName, int timeoutInSeconds, Dictionary<string, string> columnMappings)
        {
            return PerformQueryAsync(c => c.BulkInsertAsync(tableName, data, columnMapping: columnMappings, timeoutSeconds: timeoutInSeconds));
        }

        public void UpdateConfig(QueryConfiguration config)
        {
            _config = config;
        }

        private T PerformQuery<T>(Func<ISqlConnection, T> action)
        {
            return PerformQueryAsync(conn => Task.FromResult(action(conn))).Result;
        }

        private Task<T> PerformQueryAsync<T>(Func<ISqlConnection, Task<T>> action)
        {
            return _executionStrategy.ExecuteAsync(async () =>
            {
                var connectionDetails = _conn.GetConnectionDetails();
                var stopwatch = Stopwatch.StartNew();
                var plugins = _pluginManager.InstantiatePlugins();

                try
                {
                    foreach (var plugin in plugins)
                    {
                        await plugin.OnQueryExecutingAsync(connectionDetails, _metadata);
                    }

                    return await action(_conn);
                }
                finally
                {
                    stopwatch.Stop();
                    var executionDetails = new ExecutionDetails(stopwatch.Elapsed);

                    foreach (var plugin in plugins)
                    {
                        await plugin.OnQueryExecutedAsync(connectionDetails, executionDetails, _metadata);
                    }

                }
            });
        }

        private Task PerformQueryAsync(Func<ISqlConnection, Task> action)
        {
            return _executionStrategy.ExecuteAsync(async () =>
            {
                var connectionDetails = _conn.GetConnectionDetails();
                var stopwatch = Stopwatch.StartNew();
                var plugins = _pluginManager.InstantiatePlugins();

                try
                {
                    foreach (var plugin in plugins)
                    {
                        await plugin.OnQueryExecutingAsync(connectionDetails, _metadata);
                    }

                    await action(_conn);
                }
                finally
                {
                    stopwatch.Stop();
                    var executionDetails = new ExecutionDetails(stopwatch.Elapsed);

                    foreach (var plugin in plugins)
                    {
                        await plugin.OnQueryExecutedAsync(connectionDetails, executionDetails, _metadata);
                    }
                }
            });
        }
    }
}