using System.Data;
using System.Globalization;
using code.interfaces;
using code.Interfaces;

namespace code.common;

public sealed class RelationalContext<TC> : IRelationalContext<TC>
        where TC : struct
{
    private readonly IDbConnection dbConn;
    private bool connActive;

    public IDbTransaction Trn { get; private set; }
    public ILogger<RelationalContext<TC>> Logger { get; private set; }
    public IDBConcurrencyHandler<TC> ConcurrencyHandler { get; private set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public RelationalContext(IDbConnection dbConn,
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
                             ILogger<RelationalContext<TC>> logger,
                             IDBConcurrencyHandler<TC> concurrencyHandler)
    {
        this.dbConn = dbConn;
        this.Logger = logger;
        this.ConcurrencyHandler = concurrencyHandler;
        connActive = false;
    }

    public void GetTransactionContext()
    {
        if (!connActive)
        {
            dbConn.Open();
            connActive = true;
        }
        Trn = dbConn.BeginTransaction();
    }

    public void RollbackTransaction(ApplicationException ex)
    {
        if (connActive)
        {
            if (Trn != null)
            {
                Trn.Rollback();
                Logger.LogWarning("DB Transaction rolledback due to the following condition {0}", ex?.Message);
            }
        }
    }

    public void RollbackTransaction(Exception ex)
    {
        if (connActive)
        {
            if (Trn != null)
            {
                Trn.Rollback();
                Logger.LogError($"DB Transaction rolledback due to an nonexpected error {ex}");
            }
            var msg = "state conn exists?" + (dbConn != null).ToString(CultureInfo.CurrentCulture) + " dbConnState: " + (dbConn != null ? dbConn.State.ToString() : " non existent.");
            Logger.LogError(msg);
        }
    }

    public void CommitTransactionContext()
    {
        if (connActive)
        {
            Trn?.Commit();
        }
    }

    public void CloseTransactionContext()
    {
        if (connActive)
        {
            if (dbConn.State == ConnectionState.Open) dbConn.Close();
            connActive = false;
        }
    }
}