using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gossip.Connection.Fluent
{
    /// <inheritdoc cref="IBulkInsertConfigurator{T}"/>
    public class BulkInsertConfigurator<T> : IBulkInsertConfigurator<T>
    {
        private readonly IEnumerable<T> _data;
        private readonly IBulkQueryExecutor _queryExecutor;
        private string _tableName;
        private Dictionary<string, string> _columnMappings;
        private int _timeoutInSeconds = 30;

        /// <summary>
        ///     Constructs a <see cref="BulkInsertConfigurator{T}"/>.
        /// </summary>
        /// <param name="queryExecutor">The query executor, used to execute the configured bulk insert operation.</param>
        /// <param name="data">The data to insert.</param>
        public BulkInsertConfigurator(IBulkQueryExecutor queryExecutor, IEnumerable<T> data)
        {
            _data = data;
            _queryExecutor = queryExecutor;
        }

        public IBulkInsertConfigurator<T> IntoTable(string tableName)
        {
            _tableName = tableName;
            return this;
        }

        public IBulkInsertConfigurator<T> WithColumnMapping(Dictionary<string, string> columnMappings)
        {
            _columnMappings = columnMappings;
            return this;
        }

        public IBulkInsertConfigurator<T> WithTimeout(int timeoutInSeconds)
        {
            _timeoutInSeconds = timeoutInSeconds;
            return this;
        }

        public Task ExecuteAsync()
        {
            return _queryExecutor.InsertInBulkAsync(_data, _tableName, _timeoutInSeconds, _columnMappings);
        }
    }
}