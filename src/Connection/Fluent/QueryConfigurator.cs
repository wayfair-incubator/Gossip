using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Gossip.Transactions;

namespace Gossip.Connection.Fluent
{
    /// <inheritdoc cref="IQueryConfigurator"/>
    public class QueryConfigurator : IQueryConfigurator
    {
        private readonly ISqlConnection _conn;
        private readonly FunctionMetadata _metadata;
        private readonly QueryConfiguration _config;
        private readonly IQueryExecutorProvider _queryExecutorProvider;

        public QueryConfigurator(ISqlConnection conn,
            int commandTimeout,
            FunctionMetadata metadata,
            IQueryExecutorProvider queryExecutorProvider, ITransaction transaction)
        {
            _conn = conn;
            _metadata = metadata;
            _config = new QueryConfiguration
            {
                Timeout = commandTimeout,
                Transaction = transaction
            };
            _queryExecutorProvider = queryExecutorProvider;
        }

        public IQueryConfigurator WithQuery(string query)
        {
            _config.Query = query;
            return this;
        }

        public IQueryConfigurator WithParameters(object parameters)
        {
            _config.Parameters = parameters;
            return this;
        }

        public IQueryConfigurator WithTimeoutInSeconds(int timeout)
        {
            _config.Timeout = timeout;
            return this;
        }

        public IQueryConfigurator WithCancellationToken(CancellationToken cancellationToken)
        {
            _config.CancellationToken = cancellationToken;
            return this;
        }

        public IQueryConfigurator Unbuffered()
        {
            _config.Unbuffered = true;
            return this;
        }

        public IQueryExecutor Build()
        {
            return _queryExecutorProvider.GetQueryExecutor(_conn, _config, _metadata);
        }

        public IBulkInsertConfigurator<T> BulkInsert<T>(IEnumerable<T> data)
        {
            var queryExecutor = _queryExecutorProvider.GetBulkQueryExecutor(_conn, _config, _metadata);
            return new BulkInsertConfigurator<T>(queryExecutor, data);
        }
    }
}