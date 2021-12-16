using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;

namespace Gossip.Transactions
{
    [ExcludeFromCodeCoverage]
    internal class MsSqlTransactionWrapper : ITransaction
    {
        private readonly SqlTransaction _transaction;

        internal MsSqlTransactionWrapper(SqlTransaction transaction)
        {
            _transaction = transaction;
        }

        public void Dispose()
        {
            _transaction.Dispose();
        }

        public IDbTransaction Value => _transaction;

        public void Commit()
        {
            _transaction.Commit();
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }

        public void Rollback(string transactionName)
        {
            _transaction.Rollback(transactionName);
        }
    }
}