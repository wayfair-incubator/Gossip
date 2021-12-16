using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Dapper;
using Gossip.Connection;
using Gossip.Connection.Fluent;
using Gossip.Transactions;
using IsolationLevel = System.Data.IsolationLevel;

namespace Gossip.TestSupport.Adapters.Sqlite
{
    public class SqliteConnection : ISqlConnection
    {
        private readonly string _connectionString;
        private SQLiteConnection _conn;

        public SqliteConnection(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IConnectionDetails GetConnectionDetails()
        {
            return new ConnectionString
            {
                Value = _connectionString,
                Server = _connectionString
            };
        }

        public CommandDefinition CreateCommandDefinition(string query, object parameters, int? commandTimeout, CancellationToken cancellationToken, IDbTransaction transaction)
        {
            return new CommandDefinition(query, parameters, commandTimeout: commandTimeout, cancellationToken: cancellationToken, transaction: transaction);
        }

        public T QueryFirstOrDefault<T>(string query, object parameters, int commandTimeout, CancellationToken cancellationToken, IDbTransaction transaction)
        {
            return _conn.QueryFirstOrDefault<T>(CreateCommandDefinition(query, parameters, commandTimeout, cancellationToken, transaction: transaction));
        }

        public Task<T> QueryFirstOrDefaultAsync<T>(string query, object parameters, int commandTimeout, CancellationToken cancellationToken, IDbTransaction transaction)
        {
            return _conn.QueryFirstOrDefaultAsync<T>(CreateCommandDefinition(query, parameters, commandTimeout, cancellationToken, transaction: transaction));
        }

        public T QuerySingleOrDefault<T>(string query, object parameters, int commandTimeout, CancellationToken cancellationToken, IDbTransaction transaction)
        {
            return _conn.QuerySingleOrDefault<T>(CreateCommandDefinition(query, parameters, commandTimeout, cancellationToken, transaction: transaction));
        }

        public Task<T> QuerySingleOrDefaultAsync<T>(string query, object parameters, int commandTimeout, CancellationToken cancellationToken, IDbTransaction transaction)
        {
            return _conn.QuerySingleOrDefaultAsync<T>(CreateCommandDefinition(query, parameters, commandTimeout, cancellationToken, transaction: transaction));
        }

        public IEnumerable<T> Query<T>(string query, object parameters, int commandTimeout, bool buffered, IDbTransaction transaction)
        {
            return _conn.Query<T>(query, parameters, commandTimeout: commandTimeout, buffered: buffered, transaction: transaction);
        }

        public Task<IEnumerable<T>> QueryAsync<T>(string query, object parameters, int commandTimeout, CancellationToken cancellationToken, IDbTransaction transaction)
        {
            return _conn.QueryAsync<T>(CreateCommandDefinition(query, parameters, commandTimeout, cancellationToken, transaction: transaction));
        }

        public Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TReturn>(
            string query,
            Func<TFirst, TSecond, TThird, TFourth, TReturn> mapping,
            object parameters,
            int commandTimeout, IDbTransaction transaction)
        {
            return _conn.QueryAsync(query, mapping, parameters, commandTimeout: commandTimeout, transaction: transaction);
        }

        public Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(
            string query,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> mapping,
            object parameters,
            int commandTimeout, IDbTransaction transaction)
        {
            return _conn.QueryAsync(query, mapping, parameters, commandTimeout: commandTimeout, transaction: transaction);
        }

        public Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(
            string query,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> mapping,
            object parameters,
            int commandTimeout, IDbTransaction transaction)
        {
            return _conn.QueryAsync(query, mapping, parameters, commandTimeout: commandTimeout, transaction: transaction);
        }

        public Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(
            string query,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> mapping,
            object parameters,
            int commandTimeout, IDbTransaction transaction)
        {
            return _conn.QueryAsync(query, mapping, parameters, commandTimeout: commandTimeout, transaction: transaction);
        }

        public Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TReturn>(string query, Func<TFirst, TSecond, TReturn> mapping, object parameters, int commandTimeout, IDbTransaction transaction)
        {
            return _conn.QueryAsync(query, mapping, parameters, commandTimeout: commandTimeout, transaction: transaction);
        }

        public Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TReturn>(string query, Func<TFirst, TSecond, TThird, TReturn> mapping, object parameters, int commandTimeout, IDbTransaction transaction)
        {
            return _conn.QueryAsync(query, mapping, parameters, commandTimeout: commandTimeout, transaction: transaction);
        }

        public Task<int> ExecuteAsync(string query, object parameters, int commandTimeout, CancellationToken cancellationToken, IDbTransaction transaction)
        {
            return _conn.ExecuteAsync(CreateCommandDefinition(query, parameters, commandTimeout, cancellationToken, transaction: transaction));
        }

        /// <summary>
        /// Execute parameterized SQL.
        /// </summary>
        /// <param name="query">The SQL to execute for this query.</param>
        /// <param name="parameters">The parameters to use for this query.</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        /// <param name="cancellationToken">Token</param>
        /// <returns>The number of rows affected.</returns>
        public int Execute(string query, object parameters, int commandTimeout, CancellationToken cancellationToken, IDbTransaction transaction)
        {
            return _conn.Execute(CreateCommandDefinition(query, parameters, commandTimeout, cancellationToken, transaction: transaction));
        }

        public T ExecuteScalar<T>(string query, object parameters, int commandTimeout, IDbTransaction transaction)
        {
            return _conn.ExecuteScalar<T>(query, parameters, transaction, commandTimeout);
        }

        public Task<T> ExecuteScalarAsync<T>(string query, object parameters, int commandTimeout, CancellationToken cancellationToken,
            IDbTransaction transaction)
        {
            return _conn.ExecuteScalarAsync<T>(query, parameters, transaction, commandTimeout);
        }

        public async Task OpenAsync(CancellationToken cancellationToken)
        {
            _conn = new SQLiteConnection(_connectionString);
            await _conn.OpenAsync(cancellationToken);
        }

        public void Open()
        {
            OpenAsync(new CancellationToken()).GetAwaiter().GetResult();
        }

        public void Close()
        {
            _conn?.Close();
        }

        public Task BulkInsertAsync<T>(
            string insertIntoTable,
            IEnumerable<T> data,
            int batchSize = 1000,
            Action<object, SqlRowsCopiedEventArgs> notifyCallback = null,
            int notifyAfter = 10000,
            Dictionary<string, string> columnMapping = null,
            int timeoutSeconds = 30)
        {
            throw new NotImplementedException();
        }

        public void EnlistTransaction(Transaction transaction)
        {
            _conn.EnlistTransaction(transaction);
        }

        public ITransaction BeginTransaction()
        {
            return new SqliteTransactionWrapper(_conn.BeginTransaction());
        }

        public ITransaction BeginTransaction(string transactionName)
        {
            return new SqliteTransactionWrapper(_conn.BeginTransaction());
        }

        public ITransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            return new SqliteTransactionWrapper(_conn.BeginTransaction(isolationLevel));
        }

        public ITransaction BeginTransaction(IsolationLevel isolationLevel, string transactionName)
        {
            return new SqliteTransactionWrapper(_conn.BeginTransaction(isolationLevel));
        }

        public void Dispose()
        {
            this._conn?.Dispose();
            this._conn = null;
        }

        public SQLiteConnection GetSqlConnection()
        {
            return this._conn;
        }
    }

    public class SqliteTransactionWrapper : ITransaction
    {
        private readonly SQLiteTransaction _transaction;

        public SqliteTransactionWrapper(SQLiteTransaction transaction)
        {
            _transaction = transaction;
        }

        public void Dispose()
        {
            _transaction.Dispose();
        }

        public IDbTransaction Value => _transaction;

        public void Commit()
        {
            _transaction.Commit();
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }

        public void Rollback(string transactionName)
        {
            _transaction.Rollback();
        }
    }
}