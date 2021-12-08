using System.Collections.Generic;
using System.Threading;

namespace Gossip.Connection.Fluent
{
    /// <summary>
    ///     Fluent interface for configuring queries.
    /// </summary>
    public interface IQueryConfigurator
    {
        /// <summary>
        ///     Configure the query.
        /// </summary>
        /// <param name="query">The SQL query to execute.</param>
        IQueryConfigurator WithQuery(string query);

        /// <summary>
        ///     Configure the parameters for the query.
        /// </summary>
        /// <param name="parameters">The parameters to use with the query.</param>
        IQueryConfigurator WithParameters(object parameters);

        /// <summary>
        ///     Specify a query timeout in seconds.
        /// </summary>
        /// <param name="timeout">The query timeout, in seconds.</param>
        IQueryConfigurator WithTimeoutInSeconds(int timeout);

        /// <summary>
        ///     Provide a cancellation token to cooperatively handle task cancellation.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        IQueryConfigurator WithCancellationToken(CancellationToken cancellationToken);

        /// <summary>
        ///     Configure the query to execute unbuffered. If you use this, you must enumerate the results before closing the connection.
        /// </summary>
        IQueryConfigurator Unbuffered();

        /// <summary>
        ///     Build a <see cref="IQueryExecutor"/> from the configured query.
        /// </summary>
        /// <returns>A configured <see cref="IQueryExecutor"/>.</returns>
        IQueryExecutor Build();

        /// <summary>
        ///     Build a <see cref="IBulkInsertConfigurator{T}"/> from the configured query.
        /// </summary>
        /// <typeparam name="T">The type of data to bulk insert.</typeparam>
        /// <param name="data"> The data to bulk insert.</param>
        /// <returns>A configured <see cref="IBulkInsertConfigurator{T}"/></returns>
        IBulkInsertConfigurator<T> BulkInsert<T>(IEnumerable<T> data);
    }
}