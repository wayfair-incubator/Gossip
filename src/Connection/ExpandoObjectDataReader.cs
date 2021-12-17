using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Linq;

namespace Gossip.Connection
{
    /// <inheritdoc cref="IObjectDataReader"/>
    [ExcludeFromCodeCoverage]
    internal class ExpandoObjectDataReader : IObjectDataReader
    {
        private readonly IEnumerator<ExpandoObject> _dataEnumerator;
        private readonly Dictionary<string, string> _columnMapping;
        private readonly Dictionary<int, string> _columnNameLookup;
        private readonly Dictionary<string, int> _columnIndexLookup;

        public ExpandoObjectDataReader(IEnumerable<ExpandoObject> data, IDictionary<string, string> columnMapping)
        {
            _dataEnumerator = data.GetEnumerator();
            _columnNameLookup = new Dictionary<int, string>();
            _columnIndexLookup = new Dictionary<string, int>();
            _columnMapping = columnMapping != null
                ? new Dictionary<string, string>(columnMapping, StringComparer.InvariantCultureIgnoreCase)
                : new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

            if (!data.Any())
            {
                return;
            }

            Initialize(data.First());
        }

        private void Initialize(ExpandoObject item)
        {
            IDictionary<string, object> dict = item;
            var columnIndex = 1;

            foreach (var keyValuePair in dict)
            {
                if (_columnMapping.ContainsKey(keyValuePair.Key))
                {
                    continue;
                }

                _columnMapping.Add(keyValuePair.Key, keyValuePair.Key);
                _columnNameLookup.Add(columnIndex, keyValuePair.Key);
                _columnIndexLookup.Add(keyValuePair.Key, columnIndex);
                columnIndex++;
            }
        }

        public void AddMappings(SqlBulkCopyColumnMappingCollection columnMappings)
        {
            foreach (var mapping in _columnMapping)
            {
                columnMappings.Add(mapping.Key, mapping.Value);
            }
        }

        void IDataReader.Close()
        {
            throw new NotImplementedException();
        }

        DataTable IDataReader.GetSchemaTable()
        {
            throw new NotImplementedException();
        }

        bool IDataReader.NextResult()
        {
            throw new NotImplementedException();
        }

        bool IDataReader.Read()
        {
            return _dataEnumerator.MoveNext();
        }

        int IDataReader.Depth => throw new NotImplementedException();

        bool IDataReader.IsClosed => throw new NotImplementedException();

        int IDataReader.RecordsAffected => throw new NotImplementedException();

        bool IDataRecord.GetBoolean(int i)
        {
            throw new NotImplementedException();
        }

        byte IDataRecord.GetByte(int i)
        {
            throw new NotImplementedException();
        }

        long IDataRecord.GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        char IDataRecord.GetChar(int i)
        {
            throw new NotImplementedException();
        }

        long IDataRecord.GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        IDataReader IDataRecord.GetData(int i)
        {
            throw new NotImplementedException();
        }

        string IDataRecord.GetDataTypeName(int i)
        {
            throw new NotImplementedException();
        }

        DateTime IDataRecord.GetDateTime(int i)
        {
            throw new NotImplementedException();
        }

        decimal IDataRecord.GetDecimal(int i)
        {
            throw new NotImplementedException();
        }

        double IDataRecord.GetDouble(int i)
        {
            throw new NotImplementedException();
        }

        Type IDataRecord.GetFieldType(int i)
        {
            throw new NotImplementedException();
        }

        float IDataRecord.GetFloat(int i)
        {
            throw new NotImplementedException();
        }

        Guid IDataRecord.GetGuid(int i)
        {
            throw new NotImplementedException();
        }

        short IDataRecord.GetInt16(int i)
        {
            throw new NotImplementedException();
        }

        int IDataRecord.GetInt32(int i)
        {
            throw new NotImplementedException();
        }

        long IDataRecord.GetInt64(int i)
        {
            throw new NotImplementedException();
        }

        string IDataRecord.GetName(int i)
        {
            throw new NotImplementedException();
        }

        int IDataRecord.GetOrdinal(string name)
        {
            return _columnIndexLookup[name];
        }

        string IDataRecord.GetString(int i)
        {
            throw new NotImplementedException();
        }

        object IDataRecord.GetValue(int i)
        {
            var columnName = _columnNameLookup[i];
            return ((IDictionary<string, object>)_dataEnumerator.Current)[columnName];
        }

        int IDataRecord.GetValues(object[] values)
        {
            throw new NotImplementedException();
        }

        bool IDataRecord.IsDBNull(int i)
        {
            throw new NotImplementedException();
        }

        int IDataRecord.FieldCount => _columnMapping.Keys.Count;

        object IDataRecord.this[int i] => throw new NotImplementedException();

        object IDataRecord.this[string name] => throw new NotImplementedException();

        void IDisposable.Dispose()
        {
            _dataEnumerator.Dispose();
        }
    }
}