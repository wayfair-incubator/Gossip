using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Transactions;
using Gossip.Connection.Fluent;
using Gossip.Transactions;
using IsolationLevel = System.Data.IsolationLevel;

namespace Gossip.Connection
{
    /// <summary>
    /// Database Connection
    /// </summary>
    public interface IDatabaseConnection : IDisposable
    {
        /// <summary>
        /// Configures the database
        /// </summary>
        /// <param name="callerMemberName">(internal use) for logging</param>
        /// <param name="callerFilePath">(internal use) for logging</param>
        /// <returns>IQueryConfigurator</returns>
        IQueryConfigurator Configure([CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "");

        /// <summary>
        /// Execute parameterized SQL.
        /// </summary>
        /// <typeparam name="T">Generic return type</typeparam>
        /// <param name="sql">SQL query to run</param>
        /// <param name="param">Parameters to pass into the SQL query</param>
        /// <param name="callerMemberName">(internal use) for logging</param>
        /// <param name="callerFilePath">(internal use) for logging</param>
        /// <returns>SQL query result</returns>
        Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "");

        /// <summary>
        /// Execute parameterized SQL.
        /// </summary>
        /// <param name="sql">SQL query to run</param>
        /// <param name="param">Parameters to pass into the SQL query</param>
        /// <param name="callerMemberName">Caller name</param>
        /// <param name="callerFilePath">Caller location</param>
        /// <returns>Integer number of rows affected</returns>
        Task<int> ExecuteAsync(string sql, object param = null, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "");

        /// <summary>
        /// Execute parameterized SQL.
        /// </summary>
        /// <param name="sql">The SQL to execute for this query.</param>
        /// <param name="param">The parameters to use for this query.</param>
        /// <param name="callerMemberName">(internal use) for logging</param>
        /// <param name="callerFilePath">(internal use) for logging</param>
        /// <returns>The number of rows affected.</returns>
        int Execute(string sql, object param = null, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "");

        /// <summary>
        /// Execute parameterized sql
        /// </summary>
        /// <typeparam name="T">generic type</typeparam>
        /// <param name="sql">The SQL to execute for this query.</param>
        /// <param name="param">The parameters to use for this query.</param>
        /// <param name="callerMemberName">(internal use) for logging</param>
        /// <param name="callerFilePath">(internal use) for logging</param>
        /// <returns>Result of the sql query, a collection of type T</returns>
        IEnumerable<T> Query<T>(string sql, object param = null, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "");

        /// <summary>
        /// Executes Parameterized SQL
        /// </summary>
        /// <typeparam name="T">Generic Type</typeparam>
        /// <param name="sql">The SQL to execute for this query.</param>
        /// <param name="param">The parameters to use for this query.</param>
        /// <param name="callerMemberName">(internal use) for logging</param>
        /// <param name="callerFilePath">(internal use) for logging</param>
        /// <returns>The first result of the sql query</returns>
        Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "");

        /// <summary>
        /// Executes SQL
        /// </summary>
        /// <typeparam name="T">Generic Type</typeparam>
        /// <param name="sql">The SQL to execute for this query.</param>
        /// <param name="callerMemberName">(internal use) for logging</param>
        /// <param name="callerFilePath">(internal use) for logging</param>
        /// <returns>The first result of the sql query</returns>
        T QuerySingleOrDefault<T>(string sql, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "");

        /// <summary>
        /// Execute sql
        /// </summary>
        /// <typeparam name="T">generic type</typeparam>
        /// <param name="sql">The SQL to execute for this query.</param>
        /// <param name="callerMemberName">(internal use) for logging</param>
        /// <param name="callerFilePath">(internal use) for logging</param>
        /// <returns>Result of the sql query, a collection of type T</returns>
        /// <returns></returns>
        Task<T> QuerySingleOrDefaultAsync<T>(string sql, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "");

        /// <summary>
        /// Enlist a transaction
        /// </summary>
        /// <param name="transaction">transaction</param>
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