using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Gossip.Transactions;

namespace Gossip.Connection.Fluent
{
    [ExcludeFromCodeCoverage]
    public class QueryConfiguration
    {
        private ITransaction _transaction;

        public QueryConfiguration()
        {
            Transaction = new NullTransaction();
        }

        public string Query { get; set; }
        public object Parameters { get; set; }
        /// <summary>
        /// In seconds
        /// </summary>
        public int Timeout { get; set; }
        /// <summary>
        /// Whether to buffer the results in memory
        /// </summary>
        /// <remarks>This ACTUALLY only works for <code>Query{T}()</code> (not even QueryAsync) as Dapper only exposes it there</remarks>
        public bool Unbuffered { get; set; }
        public CancellationToken CancellationToken { get; set; }

        public ITransaction Transaction
        {
            get => _transaction;
            set => _transaction = value ?? new NullTransaction();
        }
    }
}