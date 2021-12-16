using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace Gossip.Connection
{
    /// <summary>
    /// SQL connection extensions
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class SqlConnectionExtensions
    {
        /// <summary>
        ///  This uses reflection to get the properties of your object.
        ///
        ///  Then transforms your objects into a DataTable
        ///
        ///  Then uses SqlBulkCopy to put it into SQL
        ///  </summary>
        /// <typeparam name="T">Generic Type</typeparam>
        /// <param name="connection">Connection</param>
        /// <param name="insertIntoTable">Table</param>
        /// <param name="data">Currently this method is stupid, the type's properties should match the database table columns, if not use the column mapping dictionary</param>
        /// <param name="batchSize">Batch size</param>
        /// <param name="notifyCallback">Callback</param>
        /// <param name="notifyAfter">Notify</param>
        /// <param name="columnMapping">Manual mapping of object property names to database column names</param>
        /// <param name="timeoutSeconds">Timeout the queries after</param>
        public static void BulkInsert<T>(
            this SqlConnection connection,
            string insertIntoTable,
            IEnumerable<T> data,
            int batchSize = 1000,
            Action<object, SqlRowsCopiedEventArgs> notifyCallback = null,
            int notifyAfter = 10000,
            Dictionary<string, string> columnMapping = null,
            int timeoutSeconds = 30)
        {
            using var reader = GetObjectDataReader(data, columnMapping);
            using var bulkCopy = new SqlBulkCopy(connection);

            bulkCopy.DestinationTableName = insertIntoTable;
            bulkCopy.BatchSize = batchSize;
            bulkCopy.BulkCopyTimeout = timeoutSeconds;

            // This is to allow arbitrary order of properties and/or the columnMapping dictionary
            reader.AddMappings(bulkCopy.ColumnMappings);

            if (notifyCallback != null)
            {
                bulkCopy.SqlRowsCopied += new SqlRowsCopiedEventHandler(notifyCallback);
                bulkCopy.NotifyAfter = notifyAfter;
            }

            bulkCopy.WriteToServer(reader);
        }

        private static IObjectDataReader GetObjectDataReader<T>(IEnumerable<T> data, Dictionary<string, string> columnMapping)
        {
            if (typeof(T) == typeof(ExpandoObject) || data.FirstOrDefault()?.GetType() == typeof(ExpandoObject))
            {
                return new ExpandoObjectDataReader(data.Cast<ExpandoObject>(), columnMapping);
            }

            return new ObjectDataReader<T>(data, columnMapping);
        }

        /// <summary>
        ///  This uses reflection to get the properties of your object.
        ///
        ///  Then transforms your objects into a DataTable
        ///
        ///  Then uses SqlBulkCopy to put it into SQL
        /// </summary>
        /// <typeparam name="T">Generic Type</typeparam>
        /// <param name="connection">Connection</param>
        /// <param name="insertIntoTable">Table</param>
        /// <param name="data">Currently this method is stupid, the type's properties should match the database table columns, if not use the column mapping dictionary</param>
        /// <param name="batchSize">Batch Size</param>
        /// <param name="notifyCallback">Callback Function</param>
        /// <param name="notifyAfter">Notify</param>
        /// <param name="columnMapping">Manual mapping of object property names to database column names</param>
        /// <param name="timeoutSeconds">Timeout the queries after</param>
        public static async Task BulkInsertAsync<T>(
            this SqlConnection connection,
            string insertIntoTable,
            IEnumerable<T> data,
            int batchSize = 1000,
            Action<object, SqlRowsCopiedEventArgs> notifyCallback = null,
            int notifyAfter = 10000,
            Dictionary<string, string> columnMapping = null,
            int timeoutSeconds = 30)
        {
            using var reader = GetObjectDataReader(data, columnMapping);
            using var bulkCopy = new SqlBulkCopy(connection);

            bulkCopy.DestinationTableName = insertIntoTable;
            bulkCopy.BatchSize = batchSize;
            bulkCopy.BulkCopyTimeout = timeoutSeconds;

            // This is to allow arbitrary order of properties and/or the columnMapping dictionary
            reader.AddMappings(bulkCopy.ColumnMappings);

            if (notifyCallback != null)
            {
                bulkCopy.SqlRowsCopied += new SqlRowsCopiedEventHandler(notifyCallback);
                bulkCopy.NotifyAfter = notifyAfter;
            }

            await bulkCopy.WriteToServerAsync(reader);
        }
    }
}