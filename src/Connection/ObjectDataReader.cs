using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace Gossip.Connection
{
    /// <summary>
    /// This wraps an <c>IEnumerable</c> of objects into a IDataReader, so that it can be used with <c>SqlBulkCopy</c>
    ///
    /// Minimal properties implemented for that singular use-case right now
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class ObjectDataReader<T> : IObjectDataReader
    {
        private readonly IEnumerator<T> _dataEnumerator;
        private readonly Dictionary<string, string> _columnMapping;

        private PropertyInfo[] _propertyArray;
        private Dictionary<string, int> _columnIdLookup;

        public ObjectDataReader(IEnumerable<T> data, IDictionary<string, string> columnMapping)
        {
            _dataEnumerator = data.GetEnumerator();
            _columnMapping = columnMapping != null
                ? new Dictionary<string, string>(columnMapping, StringComparer.InvariantCultureIgnoreCase)
                : new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

            Initialize();
        }

        private void Initialize()
        {
            var t = typeof(T);
            _propertyArray = t.GetProperties().Where(x => x.CanRead).ToArray();

            _columnIdLookup = _propertyArray.Select((p, i) => new { p.Name, Id = i })
                                            .ToDictionary(x => x.Name, x => x.Id);

            foreach (var prop in _propertyArray)
            {
                if (!_columnMapping.ContainsKey(prop.Name))
                {
                    _columnMapping.Add(prop.Name, prop.Name);
                }
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
            return _columnIdLookup[name];
        }

        string IDataRecord.GetString(int i)
        {
            throw new NotImplementedException();
        }

        object IDataRecord.GetValue(int i)
        {
            return _propertyArray[i].GetValue(_dataEnumerator.Current);
        }

        int IDataRecord.GetValues(object[] values)
        {
            throw new NotImplementedException();
        }

        bool IDataRecord.IsDBNull(int i)
        {
            throw new NotImplementedException();
        }

        int IDataRecord.FieldCount => _propertyArray.Length;

        object IDataRecord.this[int i] => throw new NotImplementedException();

        object IDataRecord.this[string name] => throw new NotImplementedException();

        void IDisposable.Dispose()
        {
            _dataEnumerator.Dispose();
        }
    }
}