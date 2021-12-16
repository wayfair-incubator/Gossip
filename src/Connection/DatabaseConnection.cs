using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Gossip.Connection.Fluent;
using Gossip.Transactions;

namespace Gossip.Connection
{
    /// <inheritdoc cref="IDatabaseConnection"/>

    /// <summary>
    /// Database Connection
    /// </summary>
    public class DatabaseConnection : IDatabaseConnection
    {
        private readonly int _commandTimeout;
        private readonly ISqlConnection _conn;
        private readonly IQueryExecutorProvider _queryExecutorProvider;
        private readonly CancellationToken _cancellationToken;
        private ITransaction _transaction;

        /// <summary>
        /// Database Connection constructor
        /// </summary>
        /// <param name="conn">Database connection</param>
        /// <param name="commandTimeout">Database Command Timeout</param>
        /// <param name="queryExecutorProvider">Query executor</param>
        /// <param name="cancellationToken">Cancellation token</param>
        public DatabaseConnection(
            ISqlConnection conn,
            int commandTimeout,
            IQueryExecutorProvider queryExecutorProvider,
            CancellationToken cancellationToken)
        {
            _conn = conn;
            _commandTimeout = commandTimeout;
            _queryExecutorProvider = queryExecutorProvider;
            _cancellationToken = cancellationToken;
        }

        /// <inheritdoc/>
        public IQueryConfigurator Configure([CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "")
        {
            var metadata = new FunctionMetadata
            {
                CallerMemberName = callerMemberName,
                CallerFilePath = callerFilePath
            };
            return new QueryConfigurator(_conn, _commandTimeout, metadata, _queryExecutorProvider, _transaction);
        }

        /// <inheritdoc/>
        public Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "")
        {
            return Configure(callerMemberName, callerFilePath)
                .WithQuery(sql)
                .WithParameters(param)
                .WithCancellationToken(_cancellationToken)
                .Build()
                .QueryAsync<T>();
        }

        /// <inheritdoc/>
        public IEnumerable<T> Query<T>(string sql, object param = null, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "")
        {
            return Configure(callerMemberName, callerFilePath)
                .WithQuery(sql)
                .WithParameters(param)
                .WithCancellationToken(_cancellationToken)
                .Build()
                .Query<T>();
        }

        /// <inheritdoc/>
        public Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "")
        {
            return Configure(callerMemberName, callerFilePath)
                .WithQuery(sql)
                .WithParameters(param)
                .WithCancellationToken(_cancellationToken)
                .Build()
                .QueryFirstOrDefaultAsync<T>();
        }

        /// <inheritdoc/>
        public int Execute(string sql, object param = null, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "")
        {
            return Configure(callerMemberName, callerFilePath)
                .WithQuery(sql)
                .WithParameters(param)
                .WithCancellationToken(_cancellationToken)
                .Build()
                .Execute();
        }

        /// <inheritdoc/>
        public T QuerySingleOrDefault<T>(string sql, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "")
        {
            return Configure(callerMemberName, callerFilePath)
                .WithQuery(sql)
                .WithCancellationToken(_cancellationToken)
                .Build()
                .QuerySingleOrDefault<T>();
        }

        /// <inheritdoc/>
        public Task<T> QuerySingleOrDefaultAsync<T>(string sql, string callerMemberName = "", string callerFilePath = "")
        {
            return Configure(callerMemberName, callerFilePath)
                .WithQuery(sql)
                .WithCancellationToken(_cancellationToken)
                .Build()
                .QuerySingleOrDefaultAsync<T>();
        }

        /// <inheritdoc/>
        public void EnlistTransaction(Transaction transaction)
        {
            _conn.EnlistTransaction(transaction);
        }

        public ITransaction BeginTransaction()
        {
            _transaction = _conn.BeginTransaction();
            return _transaction;
        }

        public ITransaction BeginTransaction(string transactionName)
        {
            _transaction = _conn.BeginTransaction(transactionName);
            return _transaction;
        }

        public ITransaction BeginTransaction(System.Data.IsolationLevel isolationLevel)
        {
            _transaction = _conn.BeginTransaction(isolationLevel);
            return _transaction;
        }

        public ITransaction BeginTransaction(System.Data.IsolationLevel isolationLevel, string transactionName)
        {
            _transaction = _conn.BeginTransaction(isolationLevel, transactionName);
            return _transaction;
        }

        /// <inheritdoc/>
        public Task<int> ExecuteAsync(string sql, object param = null, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "")
        {
            return Configure(callerMemberName, callerFilePath)
                .WithQuery(sql)
                .WithParameters(param)
                .WithCancellationToken(_cancellationToken)
                .Build()
                .ExecuteAsync();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            _conn?.Dispose();
        }
    }
}
