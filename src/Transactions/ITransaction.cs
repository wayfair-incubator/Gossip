using System;
using System.Data;

namespace Gossip.Transactions
{
    /// <summary>
    ///     Represents a transaction.
    /// </summary>
    public interface ITransaction : IDisposable
    {
        /// <summary>
        ///     The actual <see cref="IDbTransaction"/>.
        /// </summary>
        IDbTransaction Value { get; }

        /// <summary>
        ///     Commit the transaction.
        /// </summary>
        void Commit();
        
        /// <summary>
        ///     Rollback the transaction.
        /// </summary>
        void Rollback();

        /// <summary>
        ///     Rollback the named transaction.
        /// </summary>
        /// <param name="transactionName">The transaction name.</param>
        void Rollback(string transactionName);
    }
}