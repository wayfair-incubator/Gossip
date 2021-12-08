using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Gossip.Transactions;
using IsolationLevel = System.Data.IsolationLevel;

namespace Gossip.Connection.Fluent
{
    /// <summary>
    ///     Represents a SQL connection.
    /// </summary>
    public interface ISqlConnection : IDisposable
    {
        /// <summary>
        ///     Get the connection details such as the server and database name.
        /// </summary>
        /// <returns>The <see cref="IConnectionDetails"/>.</returns>
        IConnectionDetails GetConnectionDetails();

        /// <summary>
        ///     Synchronously query for the first result, or the default value of <see cref="T"/> if no result is found.
        /// </summary>
        /// <typeparam name="T">The type of results to return.</typeparam>
        /// <param name="query">The query to execute.</param>
        /// <param name="parameters">The parameters for the query.</param>
        /// <param name="commandTimeout">The effective timeout for the command.</param>
        /// <param name="cancellationToken">A cancellation token to cooperatively cancel the operation.</param>
        /// <param name="transaction">An <see cref="IDbTransaction"/> for transactional functionality.</param>
        /// <returns>The first result of the query, or the default value of <see cref="T"/> if no result is found.</returns>
        T QueryFirstOrDefault<T>(string query, object parameters, int commandTimeout, CancellationToken cancellationToken, IDbTransaction transaction);

        /// <summary>
        ///     Asynchronously query for the first result, or the default value of <see cref="T"/> if no result is found.
        /// </summary>
        /// <typeparam name="T">The type of results to return.</typeparam>
        /// <param name="query">The query to execute.</param>
        /// <param name="parameters">The parameters for the query.</param>
        /// <param name="commandTimeout">The effective timeout for the command.</param>
        /// <param name="cancellationToken">A cancellation token to cooperatively cancel the operation.</param>
        /// <param name="transaction">An <see cref="IDbTransaction"/> for transactional functionality.</param>
        /// <returns>The first result of the query, or the default value of <see cref="T"/> if no result is found.</returns>
        Task<T> QueryFirstOrDefaultAsync<T>(string query, object parameters, int commandTimeout, CancellationToken cancellationToken, IDbTransaction transaction);

        /// <summary>
        ///     Synchronously query for a single result, or the default value of <see cref="T"/> if no result is found.
        /// </summary>
        /// <typeparam name="T">The type of results to return.</typeparam>
        /// <param name="query">The query to execute.</param>
        /// <param name="parameters">The parameters for the query.</param>
        /// <param name="commandTimeout">The effective timeout for the command.</param>
        /// <param name="cancellationToken">A cancellation token to cooperatively cancel the operation.</param>
        /// <param name="transaction">An <see cref="IDbTransaction"/> for transactional functionality.</param>
        /// <returns>The result of the query, or the default value of <see cref="T"/> if no result is found.</returns>
        T QuerySingleOrDefault<T>(string query, object parameters, int commandTimeout, CancellationToken cancellationToken, IDbTransaction transaction);

        /// <summary>
        ///     Asynchronously query for a single result, or the default value of <see cref="T"/> if no result is found.
        /// </summary>
        /// <typeparam name="T">The type of results to return.</typeparam>
        /// <param name="query">The query to execute.</param>
        /// <param name="parameters">The parameters for the query.</param>
        /// <param name="commandTimeout">The effective timeout for the command.</param>
        /// <param name="cancellationToken">A cancellation token to cooperatively cancel the operation.</param>
        /// <param name="transaction">An <see cref="IDbTransaction"/> for transactional functionality.</param>
        /// <returns>The result of the query, or the default value of <see cref="T"/> if no result is found.</returns>
        Task<T> QuerySingleOrDefaultAsync<T>(string query, object parameters, int commandTimeout, CancellationToken cancellationToken, IDbTransaction transaction);

        /// <summary>
        ///     Synchronously execute the query and return the results.
        /// </summary>
        /// <typeparam name="T">The type of results to return.</typeparam>
        /// <param name="query">The query to execute.</param>
        /// <param name="parameters">The parameters for the query.</param>
        /// <param name="commandTimeout">The effective timeout for the command.</param>
        /// <param name="buffered">Whether to buffer the results in memory or not.</param>
        /// <param name="transaction">An <see cref="IDbTransaction"/> for transactional functionality.</param>
        /// <returns>The <see cref="IEnumerable{T}"/> of results.</returns>
        IEnumerable<T> Query<T>(string query, object parameters, int commandTimeout, bool buffered, IDbTransaction transaction);

        /// <summary>
        ///     Asynchronously execute the query and return the results.
        /// </summary>
        /// <typeparam name="T">The type of results to return.</typeparam>
        /// <param name="query">The query to execute.</param>
        /// <param name="parameters">The parameters for the query.</param>
        /// <param name="commandTimeout">The effective timeout for the command.</param>
        /// <param name="cancellationToken">A cancellation token to cooperatively cancel the operation.</param>
        /// <param name="transaction">An <see cref="IDbTransaction"/> for transactional functionality.</param>
        /// <returns>The <see cref="IEnumerable{T}"/> of results.</returns>
        Task<IEnumerable<T>> QueryAsync<T>(string query, object parameters, int commandTimeout, CancellationToken cancellationToken, IDbTransaction transaction);

        /// <summary>
        ///     Perform a asynchronous multi-mapping query with 2 input types. 
        ///     This returns a single type, combined from the raw types via <paramref name="mapping"/>.
        /// </summary>
        /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
        /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
        /// <typeparam name="TReturn">The combined type to return.</typeparam>
        /// <param name="query">The SQL to execute for this query.</param>
        /// <param name="mapping">The function to map row types to the return type.</param>
        /// <param name="parameters">The parameters to use for this query.</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        /// <param name="transaction">The transaction to use for this query.</param>
        /// <returns>An enumerable of <typeparamref name="TReturn"/>.</returns>
        Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TReturn>(string query, Func<TFirst, TSecond, TReturn> mapping, object parameters, int commandTimeout, IDbTransaction transaction);

        /// <summary>
        ///     Perform a asynchronous multi-mapping query with 3 input types. 
        ///     This returns a single type, combined from the raw types via <paramref name="mapping"/>.
        /// </summary>
        /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
        /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
        /// <typeparam name="TThird">The third type in the recordset.</typeparam>
        /// <typeparam name="TReturn">The combined type to return.</typeparam>
        /// <param name="query">The SQL to execute for this query.</param>
        /// <param name="mapping">The function to map row types to the return type.</param>
        /// <param name="parameters">The parameters to use for this query.</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        /// <param name="transaction">The transaction to use for this query.</param>
        /// <returns>An enumerable of <typeparamref name="TReturn"/>.</returns>
        Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TReturn>(string query, Func<TFirst, TSecond, TThird, TReturn> mapping, object parameters, int commandTimeout, IDbTransaction transaction);

        /// <summary>
        ///     Perform a asynchronous multi-mapping query with 4 input types. 
        ///     This returns a single type, combined from the raw types via <paramref name="mapping"/>.
        /// </summary>
        /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
        /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
        /// <typeparam name="TThird">The third type in the recordset.</typeparam>
        /// <typeparam name="TFourth">The fourth type in the recordset.</typeparam>
        /// <typeparam name="TReturn">The combined type to return.</typeparam>
        /// <param name="query">The SQL to execute for this query.</param>
        /// <param name="mapping">The function to map row types to the return type.</param>
        /// <param name="parameters">The parameters to use for this query.</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        /// <param name="transaction">The transaction to use for this query.</param>
        /// <returns>An enumerable of <typeparamref name="TReturn"/>.</returns>
        Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TReturn>(string query, Func<TFirst, TSecond, TThird, TFourth, TReturn> mapping, object parameters, int commandTimeout, IDbTransaction transaction);

        /// <summary>
        ///     Perform a asynchronous multi-mapping query with 5 input types. 
        ///     This returns a single type, combined from the raw types via <paramref name="mapping"/>.
        /// </summary>
        /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
        /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
        /// <typeparam name="TThird">The third type in the recordset.</typeparam>
        /// <typeparam name="TFourth">The fourth type in the recordset.</typeparam>
        /// <typeparam name="TFifth">The fifth type in the recordset.</typeparam>
        /// <typeparam name="TReturn">The combined type to return.</typeparam>
        /// <param name="query">The SQL to execute for this query.</param>
        /// <param name="mapping">The function to map row types to the return type.</param>
        /// <param name="parameters">The parameters to use for this query.</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        /// <param name="transaction">The transaction to use for this query.</param>
        /// <returns>An enumerable of <typeparamref name="TReturn"/>.</returns>
        Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(string query, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> mapping, object parameters, int commandTimeout, IDbTransaction transaction);


        /// <summary>
        ///     Perform a asynchronous multi-mapping query with 6 input types. 
        ///     This returns a single type, combined from the raw types via <paramref name="mapping"/>.
        /// </summary>
        /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
        /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
        /// <typeparam name="TThird">The third type in the recordset.</typeparam>
        /// <typeparam name="TFourth">The fourth type in the recordset.</typeparam>
        /// <typeparam name="TFifth">The fifth type in the recordset.</typeparam>
        /// <typeparam name="TSixth">The sixth type in the recordset.</typeparam>
        /// <typeparam name="TReturn">The combined type to return.</typeparam>
        /// <param name="query">The SQL to execute for this query.</param>
        /// <param name="mapping">The function to map row types to the return type.</param>
        /// <param name="parameters">The parameters to use for this query.</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        /// <param name="transaction">The transaction to use for this query.</param>
        /// <returns>An enumerable of <typeparamref name="TReturn"/>.</returns>
        Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(string query, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> mapping, object parameters, int commandTimeout, IDbTransaction transaction);

        /// <summary>
        ///     Perform a asynchronous multi-mapping query with 7 input types. 
        ///     This returns a single type, combined from the raw types via <paramref name="mapping"/>.
        /// </summary>
        /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
        /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
        /// <typeparam name="TThird">The third type in the recordset.</typeparam>
        /// <typeparam name="TFourth">The fourth type in the recordset.</typeparam>
        /// <typeparam name="TFifth">The fifth type in the recordset.</typeparam>
        /// <typeparam name="TSixth">The sixth type in the recordset.</typeparam>
        /// <typeparam name="TSeventh">The seventh type in the recordset.</typeparam>
        /// <typeparam name="TReturn">The combined type to return.</typeparam>
        /// <param name="query">The SQL to execute for this query.</param>
        /// <param name="mapping">The function to map row types to the return type.</param>
        /// <param name="parameters">The parameters to use for this query.</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        /// <param name="transaction">The transaction to use for this query.</param>
        Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(string query, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> mapping, object parameters, int commandTimeout, IDbTransaction transaction);

        /// <summary>
        ///     Asynchronously execute the query.
        /// </summary>
        /// <param name="query">The SQL to execute for this query.</param>
        /// <param name="parameters">The parameters to use for this query.</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        /// <param name="cancellationToken">A cancellation token for cooperatively cancelling the operation.</param>
        /// <param name="transaction">The transaction to use for this query.</param>
        /// <returns>The number of rows impacted by the query.</returns>
        Task<int> ExecuteAsync(string query, object parameters, int commandTimeout, CancellationToken cancellationToken, IDbTransaction transaction);

        /// <summary>
        ///     Synchronously execute the query.
        /// </summary>
        /// <param name="query">The SQL to execute for this query.</param>
        /// <param name="parameters">The parameters to use for this query.</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        /// <param name="cancellationToken">A cancellation token for cooperatively cancelling the operation.</param>
        /// <param name="transaction">The transaction to use for this query.</param>
        /// <returns>The number of rows impacted by the query.</returns>
        int Execute(string query, object parameters, int commandTimeout, CancellationToken cancellationToken, IDbTransaction transaction);

        /// <summary>
        ///     Synchronously execute the query and return a single value.
        /// </summary>
        /// <typeparam name="T">The type of data to return.</typeparam>
        /// <param name="query">The SQL to execute for this query.</param>
        /// <param name="parameters">The parameters to use for this query.</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        /// <param name="transaction">The transaction to use for this query.</param>
        /// <returns>A single <see cref="T"/> result.</returns>
        T ExecuteScalar<T>(string query, object parameters, int commandTimeout, IDbTransaction transaction);

        /// <summary>
        ///     Synchronously execute the query and return a single value.
        /// </summary>
        /// <typeparam name="T">The type of data to return.</typeparam>
        /// <param name="query">The SQL to execute for this query.</param>
        /// <param name="parameters">The parameters to use for this query.</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        /// <param name="cancellationToken">A cancellation token for cooperatively cancelling the operation.</param>
        /// <param name="transaction">The transaction to use for this query.</param>
        /// <returns>A single <see cref="T"/> result.</returns>
        Task<T> ExecuteScalarAsync<T>(string query, object parameters, int commandTimeout, CancellationToken cancellationToken, IDbTransaction transaction);

        /// <summary>
        ///     Asynchronously open the SQL connection.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token for cooperatively cancelling the operation.</param>
        Task OpenAsync(CancellationToken cancellationToken);

        /// <summary>
        ///     Synchronously open the SQL connection.
        /// </summary>
        void Open();

        /// <summary>
        ///     Synchronously close the SQL connection.
        /// </summary>
        void Close();

        /// <summary>
        ///     Bulk insert data into a table.
        /// </summary>
        /// <typeparam name="T">The type of data being bulk inserted.</typeparam>
        /// <param name="insertIntoTable">The name of the table.</param>
        /// <param name="data">The data to insert.</param>
        /// <param name="batchSize">The size of the batches for bulk insert.</param>
        /// <param name="notifyCallback">A callback to pass to <see cref="SqlRowsCopiedEventHandler"/>.</param>
        /// <param name="notifyAfter">Occurs after each batch has been processed.</param>
        /// <param name="columnMapping">A dictionary of column mappings.</param>
        /// <param name="timeoutSeconds">The timeout, specified in seconds.</param>
        Task BulkInsertAsync<T>(string insertIntoTable, IEnumerable<T> data, int batchSize = 1000, Action<object, SqlRowsCopiedEventArgs> notifyCallback = null, int notifyAfter = 10000, Dictionary<string, string> columnMapping = null, int timeoutSeconds = 30);

        /// <summary>
        ///     Enlist the given transaction.
        /// </summary>
        /// <param name="transaction">The transaction to enlist.</param>
        void EnlistTransaction(Transaction transaction);

        /// <summary>
        ///     Begin a transaction.
        /// </summary>
        ITransaction BeginTransaction();

        /// <summary>
        ///     Begin a named transaction.
        /// </summary>
        /// <param name="transactionName">The transaction name.</param>
        ITransaction BeginTransaction(string transactionName);

        /// <summary>
        ///     Begin a transaction with a specified <see cref="IsolationLevel"/>.
        /// </summary>
        /// <param name="isolationLevel">The <see cref="IsolationLevel"/> of the transaction.</param>
        ITransaction BeginTransaction(IsolationLevel isolationLevel);

        /// <summary>
        ///     Begin a named transaction with a specified <see cref="IsolationLevel"/>.
        /// </summary>
        /// <param name="isolationLevel">The <see cref="IsolationLevel"/> of the transaction.</param>
        /// <param name="transactionName">The transaction name.</param>
        ITransaction BeginTransaction(IsolationLevel isolationLevel, string transactionName);
    }
}