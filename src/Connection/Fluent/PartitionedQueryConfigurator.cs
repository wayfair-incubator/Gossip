using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Gossip.Utilities;

namespace Gossip.Connection.Fluent
{
    /// <inheritdoc cref="IPartitionConfigurator{T}"/>
    public class PartitionedQueryConfigurator<T> : IPartitionConfigurator<T>
    {
        private readonly IUpdatableQueryExecutor _executor;
        private readonly QueryConfiguration _config;
        private readonly IEnumerable<T> _partitionedBy;
        private Func<IEnumerable<T>, object> _batchParamCallback;
        private int _batchSize = 10000;

        public PartitionedQueryConfigurator(IUpdatableQueryExecutor executor, QueryConfiguration config, IEnumerable<T> partitionedBy)
        {
            _executor = executor;
            _config = config;
            _partitionedBy = partitionedBy;
        }

        public IPartitionConfigurator<T> WithBatchParamAsJsonArray()
        {
            return WithBatchParam(c => c.ToJsonArray());
        }

        public IPartitionConfigurator<T> WithBatchParam(Func<IEnumerable<T>, object> func)
        {
            _batchParamCallback = func;
            return this;
        }

        public IPartitionConfigurator<T> WithBatchSize(int batchSize = 10000)
        {
            _batchSize = batchSize;
            return this;
        }

        public async Task<IEnumerable<TResult>> QueryAsync<TResult>()
        {
            var results = new List<TResult>();

            if (!_config.Query.Contains("@batchParam"))
            {
                throw new Exception("Query must have a @batchParam variable");
            }

            foreach (var batch in _partitionedBy.Batch(_batchSize))
            {
                var newParameters = MergeParameters(_config.Parameters, batch, _batchParamCallback);
                var queryConfig = new QueryConfiguration
                {
                    Query = _config.Query,
                    Parameters = newParameters
                };
                _executor.UpdateConfig(queryConfig);

                results.AddRange(await _executor.QueryAsync<TResult>());
            }

            return results;
        }

        public IEnumerable<TResult> Query<TResult>()
        {
            var results = new List<TResult>();

            if (!_config.Query.Contains("@batchParam"))
            {
                throw new Exception("Query must have a @batchParam variable");
            }

            foreach (var batch in _partitionedBy.Batch(_batchSize))
            {
                var newParameters = MergeParameters(_config.Parameters, batch, _batchParamCallback);
                var queryConfig = new QueryConfiguration
                {
                    Query = _config.Query,
                    Parameters = newParameters
                };
                _executor.UpdateConfig(queryConfig);

                results.AddRange(_executor.Query<TResult>());
            }

            return results;
        }

        public IEnumerable<int> Execute()
        {
            var results = new List<int>();

            if (!_config.Query.Contains("@batchParam"))
            {
                throw new Exception("Query must have a @batchParam variable");
            }

            foreach (var batch in _partitionedBy.Batch(_batchSize))
            {
                var newParameters = MergeParameters(_config.Parameters, batch, _batchParamCallback);
                var queryConfig = new QueryConfiguration
                {
                    Query = _config.Query,
                    Parameters = newParameters
                };
                _executor.UpdateConfig(queryConfig);

                results.Add(_executor.Execute());
            }

            return results;
        }

        public async Task<IEnumerable<int>> ExecuteAsync()
        {
            var results = new List<int>();

            if (!_config.Query.Contains("@batchParam"))
            {
                throw new Exception("Query must have a @batchParam variable");
            }

            foreach (var batch in _partitionedBy.Batch(_batchSize))
            {
                var newParameters = MergeParameters(_config.Parameters, batch, _batchParamCallback);
                var queryConfig = new QueryConfiguration
                {
                    Query = _config.Query,
                    Parameters = newParameters
                };
                _executor.UpdateConfig(queryConfig);

                results.Add(await _executor.ExecuteAsync());
            }

            return results;
        }

        private static DynamicParameters MergeParameters<T2>(object otherParams, T2[] batch, Func<T2[], object> batchParamCallback = null)
        {
            if (batchParamCallback == null)
            {
                throw new Exception("batchParamCallback must be defined so @batchParam can have a value");
            }

            var parameters = new DynamicParameters();
            parameters.AddDynamicParams(otherParams);
            parameters.Add("@batchParam", batchParamCallback(batch));
            return parameters;
        }
    }
}