using code.interfaces;
using Dapper;

namespace code.providers.sqlServer;

public sealed class clsConcurrency<TC> : IDBConcurrencyHandler<TC>
        where TC : struct
{
    private const string columnName = "rv";

    public TC GetConcurrencyStamp(ref DynamicParameters parameters)
    {
        if (parameters == null) throw new ArgumentNullException(nameof(parameters));
        if (parameters != null)
        {
            return parameters.Get<TC>(columnName);
        }
        throw new ArgumentNullException(nameof(parameters));
    }

    public void OptimisticConcurrencyColumnAsParam(TC rowVersion, ref DynamicParameters parameters)
    {
        if (parameters == null) throw new ArgumentNullException(nameof(parameters));
        parameters.Add(columnName, rowVersion);
    }
}