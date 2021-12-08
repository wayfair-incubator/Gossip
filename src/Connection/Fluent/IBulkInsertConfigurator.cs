using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gossip.Connection.Fluent
{
    /// <summary>
    ///     Configures bulk insert operations to execute.
    /// </summary>
    /// <typeparam name="T">The type of data being bulk inserted.</typeparam>
    public interface IBulkInsertConfigurator<T>
    {
        /// <summary>
        ///     Configure which table to perform a bulk insert into.
        /// </summary>
        /// <param name="tableName">The name of the table.</param>
        IBulkInsertConfigurator<T> IntoTable(string tableName);

        /// <summary>
        ///     Configure the column mappings for the bulk insert operation.
        /// </summary>
        /// <param name="columnMappings">A dictionary of column mappings.</param>
        IBulkInsertConfigurator<T> WithColumnMapping(Dictionary<string, string> columnMappings);

        /// <summary>
        ///     Configure a timeout for the bulk insert operation.
        /// </summary>
        /// <param name="timeoutInSeconds">The timeout, specified in seconds.</param>
        IBulkInsertConfigurator<T> WithTimeout(int timeoutInSeconds);

        /// <summary>
        ///     Execute the configured bulk insert operation.
        /// </summary>
        Task ExecuteAsync();
    }
}