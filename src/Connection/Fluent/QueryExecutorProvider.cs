using Gossip.Plugins;
using Gossip.Strategies;

namespace Gossip.Connection.Fluent
{
    /// <inheritdoc cref="IQueryExecutorProvider"/>
    public class QueryExecutorProvider : IQueryExecutorProvider
    {
        private readonly IPluginManager _pluginManager;
        private readonly IExecutionStrategy _executionStrategy;

        public QueryExecutorProvider(
            IPluginManager pluginManager,
            IExecutionStrategy executionStrategy)
        {
            _pluginManager = pluginManager;
            _executionStrategy = executionStrategy;
        }

        public IBulkQueryExecutor GetBulkQueryExecutor(ISqlConnection conn, QueryConfiguration config, FunctionMetadata metadata)
        {
            return BuildQueryExecutor(conn, config, metadata);
        }

        public IQueryExecutor GetQueryExecutor(ISqlConnection conn, QueryConfiguration config, FunctionMetadata metadata)
        {
            return BuildQueryExecutor(conn, config, metadata);
        }

        private QueryExecutor BuildQueryExecutor(ISqlConnection conn, QueryConfiguration config, FunctionMetadata metadata)
        {
            return new QueryExecutor(conn, config, metadata, _pluginManager, _executionStrategy);
        }
    }
}
