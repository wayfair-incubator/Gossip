using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Gossip.Connection;
using Gossip.Connection.Fluent;
using Gossip.Transactions;
using IsolationLevel = System.Data.IsolationLevel;

namespace Gossip.TestSupport.Adapters.MockDb
{
    public class MockSqlConnection : ISqlConnection
    {
        private int _commandTimeout;

        public MockSqlConnection()
        {
            _commandTimeout = int.MaxValue;
        }

        public void Dispose()
        {
        }

        public IConnectionDetails GetConnectionDetails()
        {
            return new ConnectionString();
        }

        public T QueryFirstOrDefault<T>(string query, object parameters, int commandTimeout,
            CancellationToken cancellationToken, IDbTransaction transaction)
        {
            _commandTimeout = commandTimeout;
            return default;
        }

        public Task<T> QueryFirstOrDefaultAsync<T>(string query, object parameters, int commandTimeout, CancellationToken cancellationToken, IDbTransaction transaction)
        {
            _commandTimeout = commandTimeout;
            return default;
        }

        public T QuerySingleOrDefault<T>(string query, object parameters, int commandTimeout, CancellationToken cancellationToken, IDbTransaction transaction)
        {
            _commandTimeout = commandTimeout;
            return default;
        }

        public Task<T> QuerySingleOrDefaultAsync<T>(string query, object parameters, int commandTimeout, CancellationToken cancellationToken, IDbTransaction transaction)
        {
            _commandTimeout = commandTimeout;
            return default;
        }

        public IEnumerable<T> Query<T>(string query, object parameters, int commandTimeout, bool buffered, IDbTransaction transaction)
        {
            _commandTimeout = commandTimeout;
            return default;
        }

        public Task<IEnumerable<T>> QueryAsync<T>(string query, object parameters, int commandTimeout, CancellationToken cancellationToken, IDbTransaction transaction)
        {
            _commandTimeout = commandTimeout;
            return Task.FromResult((IEnumerable<T>)new List<T>());
        }

        public Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TReturn>(
            string query,
            Func<TFirst, TSecond, TThird, TFourth, TReturn> mapping,
            object parameters,
            int commandTimeout, 
            IDbTransaction transaction)
        {
            _commandTimeout = commandTimeout;
            return Task.FromResult((IEnumerable<TReturn>)new List<TReturn>());
        }

        public Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(
            string query,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> mapping,
            object parameters,
            int commandTimeout,
            IDbTransaction transaction)
        {
            _commandTimeout = commandTimeout;
            return Task.FromResult((IEnumerable<TReturn>)new List<TReturn>());
        }

        public Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(
            string query,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> mapping,
            object parameters,
            int commandTimeout,
            IDbTransaction transaction)
        {
            _commandTimeout = commandTimeout;
            return Task.FromResult((IEnumerable<TReturn>)new List<TReturn>());
        }

        public Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(
            string query,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> mapping,
            object parameters,
            int commandTimeout,
            IDbTransaction transaction)
        {
            _commandTimeout = commandTimeout;
            return Task.FromResult((IEnumerable<TReturn>)new List<TReturn>());
        }

        public Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TReturn>(string query, Func<TFirst, TSecond, TReturn> mapping, object parameters, int commandTimeout,
            IDbTransaction transaction)
        {
            _commandTimeout = commandTimeout;
            return Task.FromResult((IEnumerable<TReturn>)new List<TReturn>());
        }

        public Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TReturn>(string query, Func<TFirst, TSecond, TThird, TReturn> mapping, object parameters, int commandTimeout,
            IDbTransaction transaction)
        {
            _commandTimeout = commandTimeout;
            return Task.FromResult((IEnumerable<TReturn>)new List<TReturn>());
        }

        public Task<int> ExecuteAsync(string query, object parameters, int commandTimeout, CancellationToken cancellationToken,
            IDbTransaction transaction)
        {
            _commandTimeout = commandTimeout;
            return default;
        }

        public int Execute(string query, object parameters, int commandTimeout, CancellationToken cancellationToken,
            IDbTransaction transaction)
        {
            _commandTimeout = commandTimeout;
            return default;
        }

        public T ExecuteScalar<T>(string query, object parameters, int commandTimeout, IDbTransaction transaction)
        {
            return default;
        }

        public Task<T> ExecuteScalarAsync<T>(string query, object parameters, int commandTimeout, CancellationToken cancellationToken,
            IDbTransaction transaction)
        {
            return default;
        }

        public Task OpenAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public void Open()
        {
        }

        public void Close()
        {
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
            return default;
        }

        public void EnlistTransaction(Transaction transaction)
        {
        }

        public ITransaction BeginTransaction()
        {
            throw new NotImplementedException();
        }

        public ITransaction BeginTransaction(string transactionName)
        {
            throw new NotImplementedException();
        }

        public ITransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            throw new NotImplementedException();
        }

        public ITransaction BeginTransaction(IsolationLevel isolationLevel, string transactionName)
        {
            throw new NotImplementedException();
        }

        public int GetTimeout()
        {
            return _commandTimeout;
        }
    }
}