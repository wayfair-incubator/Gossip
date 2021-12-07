using System.Data;

namespace Gossip.Transactions
{
    /// <summary>
    ///     A null transaction object.
    /// </summary>
    /// <inheritdoc cref="ITransaction"/>
    public class NullTransaction : ITransaction
    {
        public void Dispose()
        {
            
        }

        public IDbTransaction Value => null;

        public void Commit()
        {
            
        }

        public void Rollback()
        {
            
        }

        public void Rollback(string transactionName)
        {
            
        }
    }
}