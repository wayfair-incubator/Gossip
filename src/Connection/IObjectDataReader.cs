using System.Data;
using System.Data.SqlClient;

namespace Gossip.Connection
{
    /// <summary>
    /// This wraps an <c>IEnumerable</c> of objects into a IDataReader, so that it can be used with <c>SqlBulkCopy</c>
    ///
    /// Minimal properties implemented for that singular use-case right now
    /// </summary>
    internal interface IObjectDataReader : IDataReader
    {
        /// <summary>
        ///     Add SQL column mappings for bulk copy operations.
        /// </summary>
        /// <param name="columnMappings">The column mappings to add.</param>
        void AddMappings(SqlBulkCopyColumnMappingCollection columnMappings);
    }
}