using System.Data;
using System.Data.SqlClient;
using code.interfaces;
using code.Interfaces;
using Dapper;

namespace code.common;

public abstract class DataAccess<TEntity, TKey, TC>
        where TEntity : RelationalEntity<TKey, TC>, new()
        where TKey : IEquatable<TKey>
        where TC : struct
{
    internal IRelationalContext<TC> rkm;
    internal IEnumerable<TEntity>? cache;
    protected virtual ISQLData queries { get; private set; }
    internal DynamicParameters? DBParams;
    internal readonly ILogger<DataAccess<TEntity, TKey, TC>> logger;

    protected DataAccess(IRelationalContext<TC> rkm,
                         ISQLData queries,
                         ILogger<DataAccess<TEntity, TKey, TC>> logger)
    {
        this.rkm = rkm;
        this.queries = queries;
        this.logger = logger;
        this.cache = null;
    }

    protected bool CachedData
    {
        get
        {
            return (cache != null) && (cache.Any() == true);
        }
    }

    protected abstract DynamicParameters KeyAsParams(TKey key);
    protected abstract DynamicParameters FieldsAsParams(TEntity entity); //Populate DBParams with all fields in the table

    protected async Task<IEnumerable<T>?> Get<T>(DynamicParameters? param, string query, CommandType commandType = CommandType.Text)
    {
        try
        {
            if (rkm != null && rkm.Trn != null && rkm.Trn.Connection != null)
            {
                return await rkm.Trn.Connection.QueryAsync<T>(query, param, rkm.Trn, null, commandType).ConfigureAwait(false);
            }
            return null;
        }
        catch (SqlException ex)
        {
            logger.LogError($"Error al hacer SQL-GET<T> {query} Error: {ex}");
            throw;
        }
        catch (TimeoutException ex)
        {
            logger.LogError($"Timeout al hacer SQL-GET<T> {query} Error: {ex}");
            throw;
        }
    }

    protected async Task<IEnumerable<TEntity>?> Get(DynamicParameters param, string query)
    {
        return await Get<TEntity>(param, query).ConfigureAwait(false);
    }

    protected async Task<IEnumerable<TEntity>?> GetALL(DynamicParameters? param)
    {
        return await Get<TEntity>(param, queries.SQLGetAll).ConfigureAwait(false);
    }

    protected async Task<TEntity?> GetEntity(TKey key, bool UseCache = true)
    {
        TEntity? result;
        if (UseCache == true && cache != null)
        {
            result = cache.Where(x => x.Key.Equals(key)).FirstOrDefault();
            if (result != null) return result;
        }
        var p = KeyAsParams(key);
        var data = await Get(p, queries.SQLDataEntity).ConfigureAwait(false);
        cache = data;
        if (data != null && data.Any())
        {
            return data.First();
        }
        return null;
    }

    protected async Task<TResult?> Add<TResult>(DynamicParameters param)
    {
        if (rkm != null && rkm.Trn != null && rkm.Trn.Connection != null)
        {
            try
            {
                var result = await rkm.Trn.Connection.QueryAsync<TResult>(sql: queries.NewDataEntity,
                         param: param,
                         transaction: rkm.Trn,
                         commandType: CommandType.Text).ConfigureAwait(false);
                if (result != null)
                {
                    return result.FirstOrDefault();
                }
                return default;
            }
            catch (SqlException ex)
            {
                logger.LogError($"Error al hacer SQL-ADD<TResult> {queries.NewDataEntity} Error:{ex}");
                throw;
            }
            catch (TimeoutException ex)
            {
                logger.LogError($"Error al hacer SQL-ADD<TResult> (TimeOut) {queries.NewDataEntity} Error:{ex}");
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError($"Error general al hacer SQL-ADD<TResult> {queries.NewDataEntity} Error:{ex}");
                throw;
            }
        }
        else
        {
            throw new Exception("Connection not ready to perform an INSERT operation");
        }
    }

    protected async Task<bool> Add(DynamicParameters param)
    {
        if (rkm != null && rkm.Trn != null && rkm.Trn.Connection != null)
        {
            try
            {
                var result = await rkm.Trn.Connection.QueryAsync<TEntity>(sql: queries.NewDataEntity,
                                                         param: param,
                                                         transaction: rkm.Trn,
                                                         commandType: CommandType.Text).ConfigureAwait(false);
                if (result != null && result.Any() == true)
                {
                    if (cache == null)
                    {
                        cache = result;
                    }
                    else
                    {
                        cache.ToList().AddRange(result);
                    }
                    return true;
                }
                return false;
            }
            catch (SqlException ex)
            {
                logger.LogError($"Error al hacer SQL-ADD<bool> {queries.NewDataEntity} Error:{ex}");
                throw;
            }
            catch (TimeoutException ex)
            {
                logger.LogError($"Error al hacer SQL-ADD<bool> (TimeOut) {queries.NewDataEntity} Error:{ex}");
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError($"Error general al hacer SQL-ADD<bool> {queries.NewDataEntity} Error:{ex}");
                throw;
            }
        }
        else
        {
            throw new Exception("Connection not ready to perform an INSERT operation");
        }
    }

    protected async Task Add(TEntity item)
    {
        if (rkm != null && rkm.Trn != null && rkm.Trn.Connection != null)
        {
            try
            {
                DBParams = FieldsAsParams(item);
                var result = await rkm.Trn.Connection.ExecuteAsync(queries.NewDataEntity, DBParams, rkm.Trn, null, CommandType.Text).ConfigureAwait(false);
                if (result != 1)
                {
                    throw new DBConcurrencyException("Error de concurrencia al hacer INSERT en query " + queries.NewDataEntity);
                }
            }
            catch (SqlException ex)
            {
                logger.LogError($"Error al hacer SQL-ADD<bool> {queries.NewDataEntity} Error: {ex}");
                throw;
            }
            catch (TimeoutException ex)
            {
                logger.LogError($"Timeout al hacer SQL-ADD<bool> {queries.NewDataEntity} Error: {ex}");
                throw;
            }
        }
        else
        {
            throw new Exception("Connection not ready to perform a transactional operation");
        }
    }

    protected async Task<bool> Delete(DynamicParameters param)
    {
        if (rkm != null && rkm.Trn != null && rkm.Trn.Connection != null)
        {
            try
            {
                var result = await rkm.Trn.Connection.ExecuteAsync(sql: queries.DeleteDataEntity,
                                                                   param: param,
                                                                   transaction: rkm.Trn,
                                                                   commandType: CommandType.Text).ConfigureAwait(false);
                return result == 1;
            }
            catch (SqlException ex)
            {
                logger.LogError($"Error al hacer SQL-DELETE {queries.DeleteDataEntity} Error:{ex}");
                throw;
            }
            catch (TimeoutException ex)
            {
                logger.LogError($"Error al hacer SQL-DELETE (TimeOut) {queries.DeleteDataEntity} Error:{ex}");
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError($"Error general al hacer SQL-DELETE {queries.DeleteDataEntity} Error:{ex}");
                throw;
            }
        }
        else
        {
            throw new Exception("Connection not ready to perform a transactional operation");
        }
    }

    protected async Task Delete(TKey key, TC? RowVersion)
    {
        var p = KeyAsParams(key);
        await Set(p, RowVersion, queries.DeleteDataEntity).ConfigureAwait(false);
    }

    protected async Task SetAll(TEntity item, TC? RowVersion)
    {
        var p = FieldsAsParams(item);
        await Set(p, RowVersion, queries.UpdateWholeEntity).ConfigureAwait(false);
    }

    protected async Task<TResult> Set<TResult>(DynamicParameters param, TC? rowVersion, string query, Action<TResult> setFields)
    {
        if (rkm != null && rkm.Trn != null && rkm.Trn.Connection != null)
        {
            try
            {
                if (rowVersion.HasValue)
                {
                    rkm.ConcurrencyHandler.OptimisticConcurrencyColumnAsParam(rowVersion.Value, ref param);
                }
                var x = await rkm.Trn.Connection.QueryAsync<TResult>(sql: query, param: param,
                                                                     transaction: rkm.Trn,
                                                                     commandType: CommandType.Text).ConfigureAwait(false);
                if (x != null && x.Any())
                {
                    if (CachedData == true)
                    {
                        setFields?.Invoke(x.First());
                    }
                    return x.First();
                }
                throw new DBConcurrencyException("Error de concurrencia en la base de datos");
            }
            catch (SqlException ex)
            {
                logger.LogError($"Error al hacer SQL-SET {query} Error:{ex}");
                throw;
            }
            catch (TimeoutException ex)
            {
                logger.LogError($"Error al hacer SQL-SET (TimeOut) {query} Error:{ex}");
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError($"Error general al hacer SQL-SET {query} Error:{ex}");
                throw;
            }
        }
        else
        {
            throw new Exception("Connection not ready to perform a transactional operation");
        }
    }

    protected async Task<bool> Set(DynamicParameters param, TC? rowVersion, string query, Action? setFields)
    {
        if (rkm != null && rkm.Trn != null && rkm.Trn.Connection != null)
        {
            try
            {
                if (rowVersion.HasValue)
                {
                    rkm.ConcurrencyHandler.OptimisticConcurrencyColumnAsParam(rowVersion.Value, ref param);
                }
                var x = await rkm.Trn.Connection.ExecuteAsync(sql: query,
                                                              param: param,
                                                              transaction: rkm.Trn,
                                                              commandType: CommandType.Text).ConfigureAwait(false);
                if (x > 0)
                {
                    if (CachedData == true)
                    {
                        setFields?.Invoke();
                    }
                }
                else
                {
                    throw new DBConcurrencyException("Error de concurrencia en la base de datos");
                }
                return true;
            }
            catch (SqlException ex)
            {
                logger.LogError($"Error al hacer SQL-SET {query} Error:{ex}");
                throw;
            }
            catch (TimeoutException ex)
            {
                logger.LogError($"Error al hacer SQL-SET (TimeOut) {query} Error:{ex}");
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError($"Error general al hacer SQL-SET {query} Error:{ex}");
                throw;
            }
        }
        else
        {
            throw new Exception("Connection not ready to perform a transactional operation");
        }
    }

    protected async Task Set(DynamicParameters param, TC? rowVersion, string query)
    {
        await Set(param, rowVersion, query, null).ConfigureAwait(false);
    }

    /// <summary>
    /// Update whole entity
    /// </summary>
    /// <param name="param"></param>
    /// <param name="rowVersion"></param>
    /// <returns></returns>
    protected async Task Set(DynamicParameters param, TC? rowVersion)
    {
        await Set(param, rowVersion, queries.UpdateWholeEntity, null).ConfigureAwait(false);
    }
}