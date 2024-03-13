using System.Data;
using code.interfaces;

namespace code.Interfaces;

public interface IRelationalContext<TC>
        where TC : struct
{
    IDbTransaction Trn { get; }
    IDBConcurrencyHandler<TC> ConcurrencyHandler { get; }
    void GetTransactionContext();
    void CommitTransactionContext();
    void CloseTransactionContext();
    void RollbackTransaction(Exception ex);
    void RollbackTransaction(ApplicationException ex);
}