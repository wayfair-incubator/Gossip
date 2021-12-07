using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gossip.Connection.Fluent
{
    /// <summary>
    ///     Configures partitions to be executed upon for batch operations.
    /// </summary>
    /// <typeparam name="T">The batch parameter data type.</typeparam>
    public interface IPartitionConfigurator<out T>
    {
        /// <summary>
        ///     Query synchronously for the results of the configured batch operation.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="TResult"/>.</returns>
        IEnumerable<TResult> Query<TResult>();

        /// <summary>
        ///     Query asynchronously for the results of the configured batch operation.
        /// </summary>
        /// <typeparam name="TResult">The data type of the expected result.</typeparam>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="TResult"/>.</returns>
        Task<IEnumerable<TResult>> QueryAsync<TResult>();

        /// <summary>
        ///      Execute synchronously the configured batched operation.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> of row counts effected in each batch.</returns>
        IEnumerable<int> Execute();

        /// <summary>
        ///      Execute asynchronously the configured batched operation.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> of row counts effected in each batch.</returns>
        Task<IEnumerable<int>> ExecuteAsync();

        /// <summary>
        ///     Convert batched objects to sql parameter @batchParam.
        /// </summary>
        /// <param name="func">A batch of the objects to be batched. Function should return sql that replaces @batchParam.</param>
        IPartitionConfigurator<T> WithBatchParam(Func<IEnumerable<T>, object> func);


        /// <summary>
        ///     The parameter to configure batched queries from.
        /// </summary>
        IPartitionConfigurator<T> WithBatchParamAsJsonArray();

        /// <summary>
        ///     Configure the batch size for the operation.
        /// </summary>
        /// <param name="batchSize">The batch size to use for the operation.</param>
        IPartitionConfigurator<T> WithBatchSize(int batchSize = 10000);
    }
}