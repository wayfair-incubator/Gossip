using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Dapper;
using Gossip.Connection.Fluent;
using Gossip.Transactions;

namespace Gossip.Connection
{
    /// <inheritdoc cref="ISqlConnection"/>

    /// <summary>
    /// SQL connection wrapper
    /// </summary>
    public class SqlConnectionWrapper : ISqlConnection
    {
        private readonly SqlConnection _conn;
        private readonly IConnectionString _connectionString;

        /// <summary>
        /// SQL connection wrapper constructor
        /// </summary>
        /// <param name="connectionString">Database connection string</param>
        public SqlConnectionWrapper(IConnectionString connectionString)
        {
            _conn = new SqlConnection(connectionString.Value);
            _connectionString = connectionString;
        }

        /// <inheritdoc/>
        public IConnectionDetails GetConnectionDetails()
        {
            return _connectionString;
        }

        /// <summary>
        /// Creates Command Definition
        /// </summary>
        /// <param name="query">SQL command</param>
        /// <param name="parameters">Parameters to pass into SQL command</param>
        /// <param name="commandTimeout">SQL command timeout</param>
        /// <param name="cancellationToken">cancellation token</param>
        /// <param name="transaction">The transaction</param>
        /// <returns>Command Definition</returns>
        public CommandDefinition CreateCommandDefinition(string query, object parameters, int? commandTimeout, CancellationToken cancellationToken, IDbTransaction transaction)
        {
            return new CommandDefinition(query, parameters, commandTimeout: commandTimeout, cancellationToken: cancellationToken, transaction: transaction);
        }

        /// <inheritdoc/>
        public T QueryFirstOrDefault<T>(string query, object parameters, int commandTimeout, CancellationToken cancellationToken, IDbTransaction transaction)
        {
            return _conn.QueryFirstOrDefault<T>(CreateCommandDefinition(query, parameters, commandTimeout, cancellationToken, transaction));
        }

        /// <inheritdoc/>
        public Task<T> QueryFirstOrDefaultAsync<T>(string query, object parameters, int commandTimeout, CancellationToken cancellationToken, IDbTransaction transaction)
        {
            return _conn.QueryFirstOrDefaultAsync<T>(CreateCommandDefinition(query, parameters, commandTimeout, cancellationToken, transaction));
        }

        /// <inheritdoc/>
        public T QuerySingleOrDefault<T>(string query, object parameters, int commandTimeout, CancellationToken cancellationToken, IDbTransaction transaction)
        {
            return _conn.QuerySingleOrDefault<T>(CreateCommandDefinition(query, parameters, commandTimeout, cancellationToken, transaction));
        }

        /// <inheritdoc/>
        public Task<T> QuerySingleOrDefaultAsync<T>(string query, object parameters, int commandTimeout, CancellationToken cancellationToken, IDbTransaction transaction)
        {
            return _conn.QuerySingleOrDefaultAsync<T>(CreateCommandDefinition(query, parameters, commandTimeout, cancellationToken, transaction));
        }

        public T ExecuteScalar<T>(string query, object parameters, int commandTimeout, IDbTransaction transaction)
        {
            return _conn.ExecuteScalar<T>(CreateCommandDefinition(query, parameters, commandTimeout, CancellationToken.None, transaction));
        }

        public Task<T> ExecuteScalarAsync<T>(string query, object parameters, int commandTimeout, CancellationToken cancellationToken, IDbTransaction transaction)
        {
            return _conn.ExecuteScalarAsync<T>(CreateCommandDefinition(query, parameters, commandTimeout, cancellationToken, transaction));
        }

        /// <inheritdoc/>
        public IEnumerable<T> Query<T>(string query, object parameters, int commandTimeout, bool buffered, IDbTransaction transaction)
        {
            return _conn.Query<T>(query, parameters, commandTimeout: commandTimeout, buffered: buffered, transaction: transaction);
        }

        /// <inheritdoc/>
        public Task<IEnumerable<T>> QueryAsync<T>(string query, object parameters, int commandTimeout, CancellationToken cancellationToken, IDbTransaction transaction)
        {
            return _conn.QueryAsync<T>(CreateCommandDefinition(query, parameters, commandTimeout, cancellationToken, transaction));
        }

        /// <inheritdoc/>
        public Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TReturn>(string query, Func<TFirst, TSecond, TReturn> mapping, object parameters, int commandTimeout, IDbTransaction transaction)
        {
            return _conn.QueryAsync(query, mapping, parameters, commandTimeout: commandTimeout, transaction: transaction);
        }

        /// <inheritdoc/>
        public Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TReturn>(string query, Func<TFirst, TSecond, TThird, TReturn> mapping, object parameters, int commandTimeout, IDbTransaction transaction)
        {
            return _conn.QueryAsync(query, mapping, parameters, commandTimeout: commandTimeout, transaction: transaction);
        }

        /// <inheritdoc/>
        public Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TReturn>(string query, Func<TFirst, TSecond, TThird, TFourth, TReturn> mapping, object parameters, int commandTimeout, IDbTransaction transaction)
        {
            return _conn.QueryAsync(query, mapping, parameters, commandTimeout: commandTimeout, transaction: transaction);
        }

        /// <inheritdoc/>
        public Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(string query, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> mapping, object parameters, int commandTimeout, IDbTransaction transaction)
        {
            return _conn.QueryAsync(query, mapping, parameters, commandTimeout: commandTimeout, transaction: transaction);
        }

        /// <inheritdoc/>
        public Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(string query, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> mapping, object parameters, int commandTimeout, IDbTransaction transaction)
        {
            return _conn.QueryAsync(query, mapping, parameters, commandTimeout: commandTimeout, transaction: transaction);
        }

        /// <inheritdoc/>
        public Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(string query, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> mapping, object parameters, int commandTimeout, IDbTransaction transaction)
        {
            return _conn.QueryAsync(query, mapping, parameters, commandTimeout: commandTimeout, transaction: transaction);
        }

        /// <inheritdoc/>
        public Task<int> ExecuteAsync(string query, object parameters, int commandTimeout, CancellationToken cancellationToken, IDbTransaction transaction)
        {
            return _conn.ExecuteAsync(CreateCommandDefinition(query, parameters, commandTimeout, cancellationToken, transaction));
        }

        /// <inheritdoc/>
        public int Execute(string query, object parameters, int commandTimeout, CancellationToken cancellationToken, IDbTransaction transaction)
        {
            return _conn.Execute(CreateCommandDefinition(query, parameters, commandTimeout, cancellationToken, transaction));
        }

        /// <inheritdoc/>
        public Task OpenAsync(CancellationToken cancellationToken)
        {
            return _conn.OpenAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public void Open()
        {
            _conn.Open();
        }

        /// <inheritdoc/>
        public void Close()
        {
            _conn.Close();
        }

        /// <inheritdoc/>
        public Task BulkInsertAsync<T>(
            string insertIntoTable,
            IEnumerable<T> data,
            int batchSize = 1000,
            Action<object, SqlRowsCopiedEventArgs> notifyCallback = null,
            int notifyAfter = 10000,
            Dictionary<string, string> columnMapping = null,
            int timeoutSeconds = 30)
        {
            return _conn.BulkInsertAsync(insertIntoTable, data, batchSize, notifyCallback, notifyAfter, columnMapping, timeoutSeconds);
        }

        /// <inheritdoc/>
        public void EnlistTransaction(Transaction transaction)
        {
            _conn.EnlistTransaction(transaction);
        }

        public ITransaction BeginTransaction()
        {
            return new MsSqlTransactionWrapper(_conn.BeginTransaction());
        }

        public ITransaction BeginTransaction(string transactionName)
        {
            return new MsSqlTransactionWrapper(_conn.BeginTransaction(transactionName));
        }

        public ITransaction BeginTransaction(System.Data.IsolationLevel isolationLevel)
        {
            return new MsSqlTransactionWrapper(_conn.BeginTransaction(isolationLevel));
        }

        public ITransaction BeginTransaction(System.Data.IsolationLevel isolationLevel, string transactionName)
        {
            return new MsSqlTransactionWrapper(_conn.BeginTransaction(isolationLevel, transactionName));
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            _conn?.Dispose();
        }

        /// <summary>
        /// Gets sql connection
        /// </summary>
        /// <returns>SqlConnection</returns>
        public SqlConnection GetSqlConnection()
        {
            return this._conn;
        }
    }
}