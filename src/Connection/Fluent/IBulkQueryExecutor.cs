using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gossip.Connection.Fluent
{
    /// <summary>
    ///     Executes a bulk insert operation.
    /// </summary>
    public interface IBulkQueryExecutor
    {
        /// <summary>
        ///     Execute a bulk insert operation into the specified table.
        /// </summary>
        /// <typeparam name="T">The type of data being bulk inserted.</typeparam>
        /// <param name="data">The data to bulk insert.</param>
        /// <param name="tableName">The name of the table.</param>
        /// <param name="timeoutInSeconds">The timeout, specified in seconds.</param>
        /// <param name="columnMappings">A dictionary of column mappings.</param>
        /// <returns></returns>
        Task InsertInBulkAsync<T>(IEnumerable<T> data, string tableName, int timeoutInSeconds, Dictionary<string, string> columnMappings);
    }
}