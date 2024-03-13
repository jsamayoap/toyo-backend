using Dapper;

namespace code.interfaces;

public interface IDBConcurrencyHandler<TC>
        where TC : struct
{
    void OptimisticConcurrencyColumnAsParam(TC rowVersion, ref DynamicParameters parameters);
    TC GetConcurrencyStamp(ref DynamicParameters parameters);
}