using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gossip.Connection.Fluent
{
    /// <summary>
    ///     Executes configured queries.
    /// </summary>
    public interface IQueryExecutor
    {
        /// <summary>
        ///     Synchronously query for the first result, or the default value of <see cref="T"/> if no result is found.
        /// </summary>
        /// <typeparam name="T">The data type of the returned value.</typeparam>
        /// <returns>The first result of the query, or the default value of <see cref="T"/> if no result is found.</returns>
        T QueryFirstOrDefault<T>();

        /// <summary>
        ///     Asynchronously query for the first result, or the default value of <see cref="T"/> if no result is found.
        /// </summary>
        /// <typeparam name="T">The data type of the returned value.</typeparam>
        /// <returns>The first result of the query, or the default value of <see cref="T"/> if no result is found.</returns>
        Task<T> QueryFirstOrDefaultAsync<T>();

        /// <summary>
        ///     Synchronously query for a single result, or the default value of <see cref="T"/> if no result is found.
        /// </summary>
        /// <typeparam name="T">The data type of the returned value.</typeparam>
        /// <returns>The result of the query, or the default value of <see cref="T"/> if no result is found.</returns>
        T QuerySingleOrDefault<T>();

        /// <summary>
        ///     Asynchronously query for a single result, or the default value of <see cref="T"/> if no result is found.
        /// </summary>
        /// <typeparam name="T">The data type of the returned value.</typeparam>
        /// <returns>The result of the query, or the default value of <see cref="T"/> if no result is found.</returns>
        Task<T> QuerySingleOrDefaultAsync<T>();

        /// <summary>
        ///     Synchronously execute the query and return the results.
        /// </summary>
        /// <typeparam name="T">The data type of the returned values.</typeparam>
        /// <returns>The <see cref="IEnumerable{T}"/> of results.</returns>
        IEnumerable<T> Query<T>();

        /// <summary>
        ///     Asynchronously execute the query and return the results.
        /// </summary>
        /// <typeparam name="T">The data type of the returned values.</typeparam>
        /// <returns>The <see cref="IEnumerable{T}"/> of results.</returns>
        Task<IEnumerable<T>> QueryAsync<T>();

        /// <summary>
        ///     Perform an asynchronous multi-mapping query with 2 input types. 
        ///     This returns a single type, combined from the raw types via <paramref name="mapping"/>.
        /// </summary>
        /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
        /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
        /// <typeparam name="TReturn">The combined type to return.</typeparam>
        /// <param name="mapping">The function to map row types to the return type.</param>
        /// <returns>An enumerable of <typeparamref name="TReturn"/>.</returns>
        Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TReturn>(Func<TFirst, TSecond, TReturn> mapping);

        ///<summary>
        ///     Perform an asynchronous multi-mapping query with 3 input types. 
        ///     This returns a single type, combined from the raw types via <paramref name="mapping"/>.
        /// </summary>
        /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
        /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
        /// <typeparam name="TThird">The third type in the recordset.</typeparam>
        /// <typeparam name="TReturn">The combined type to return.</typeparam>
        /// <param name="mapping">The function to map row types to the return type</param>
        /// <returns>An enumerable of <typeparamref name="TReturn"/>.</returns>
        Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TReturn>(Func<TFirst, TSecond, TThird, TReturn> mapping);

        /// <summary>
        ///     Perform an asynchronous multi-mapping query with 4 input types. 
        ///     This returns a single type, combined from the raw types via <paramref name="mapping"/>.
        /// </summary>
        /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
        /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
        /// <typeparam name="TThird">The third type in the recordset.</typeparam>
        /// <typeparam name="TFourth">The fourth type in the recordset.</typeparam>
        /// <typeparam name="TReturn">The combined type to return.</typeparam>
        /// <param name="mapping">The function to map row types to the return type.</param>
        /// <returns>An enumerable of <typeparamref name="TReturn"/>.</returns>
        Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TReturn>(Func<TFirst, TSecond, TThird, TFourth, TReturn> mapping);

        /// <summary>
        ///     Perform an asynchronous multi-mapping query with 5 input types. 
        ///     This returns a single type, combined from the raw types via <paramref name="mapping"/>.
        /// </summary>
        /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
        /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
        /// <typeparam name="TThird">The third type in the recordset.</typeparam>
        /// <typeparam name="TFourth">The fourth type in the recordset.</typeparam>
        /// <typeparam name="TFifth">The fifth type in the recordset.</typeparam>
        /// <typeparam name="TReturn">The combined type to return.</typeparam>
        /// <param name="mapping">The function to map row types to the return type.</param>
        /// <returns>An enumerable of <typeparamref name="TReturn"/>.</returns>
        Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> mapping);

        /// <summary>
        ///     Perform an asynchronous multi-mapping query with 6 input types. 
        ///     This returns a single type, combined from the raw types via <paramref name="mapping"/>.
        /// </summary>
        /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
        /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
        /// <typeparam name="TThird">The third type in the recordset.</typeparam>
        /// <typeparam name="TFourth">The fourth type in the recordset.</typeparam>
        /// <typeparam name="TFifth">The fifth type in the recordset.</typeparam>
        /// <typeparam name="TSixth">The sixth type in the recordset.</typeparam>
        /// <typeparam name="TReturn">The combined type to return.</typeparam>
        /// <param name="mapping">The function to map row types to the return type.</param>
        /// <returns>An enumerable of <typeparamref name="TReturn"/>.</returns>
        Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> mapping);

        /// <summary>
        ///     Perform an asynchronous multi-mapping query with 7 input types. 
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
        /// <param name="mapping">The function to map row types to the return type.</param>
        /// <returns>An enumerable of <typeparamref name="TReturn"/>.</returns>
        Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> mapping);

        /// <summary>
        ///     Perform the query partitioned into batches on the <paramref name="partitionedBy"/> parameter.
        /// </summary>
        /// <typeparam name="T">The type of data to perform partitioned batch queries on.</typeparam>
        /// <param name="partitionedBy">The data to perform partitioned batch queries on.</param>
        /// <returns></returns>
        IPartitionConfigurator<T> BatchedBy<T>(IEnumerable<T> partitionedBy);

        /// <summary>
        ///     Asynchronously execute the query.
        /// </summary>
        /// <returns>The number of rows impacted by the query.</returns>
        Task<int> ExecuteAsync();

        /// <summary>
        ///     Synchronously execute the query and return a single value.
        /// </summary>
        /// <typeparam name="T">The type of data to return.</typeparam>
        /// <returns>A single <see cref="T"/> result.</returns>
        T ExecuteScalar<T>();

        /// <summary>
        ///     Asynchronously execute the query and return a single value.
        /// </summary>
        /// <typeparam name="T">The type of data to return.</typeparam>
        /// <returns>A single <see cref="T"/> result.</returns>
        Task<T> ExecuteScalarAsync<T>();

        /// <summary>
        ///     Synchronously execute the query.
        /// </summary>
        /// <returns>The number of rows impacted by the query.</returns>
        int Execute();
    }
}