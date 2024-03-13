using code.common;
using code.entityModels;
using code.interfaces;
using code.Interfaces;
using code.models;
using code.repositories.services;
using Dapper;

namespace code.repositories.impl;

public sealed class DoctorRepository<TI, TC> : DataAccess<DoctorEntityModel<TI, TC>, TI, TC>, IDoctorRepository<TI, TC>
        where TI : struct, IEquatable<TI>
        where TC : struct
{
    public DoctorRepository(IRelationalContext<TC> rkm,
                            ISQLData queries,
                            ILogger<DoctorRepository<TI, TC>> logger) : base(rkm, queries, logger)
    {
    }

    public async Task<TI> AddDoctor(DoctorModel<TI> doctor)
    {
        var p = new DynamicParameters();
        p.Add("NOMBRES", doctor.Nombres);
        p.Add("APELLIDO_PATERNO", doctor.Apellido_paterno);
        p.Add("APELLIDO_MATERNO", doctor.Apellido_materno);
        p.Add("FECHA_NACIMIENTO", doctor.Fecha_nacimiento);
        p.Add("ESPECIALIDAD", doctor.Especialidad);
        return await Add<TI>(p).ConfigureAwait(false);
    }

    public Task DeleteDoctor(TI id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<DoctorEntityModel<TI, TC>>> GetDoctor(TI doctorID)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<DoctorEntityModel<TI, TC>>?> GetDoctor()
    {
        return await GetALL(null).ConfigureAwait(false);
    }

    public Task UpdateDoctor(DoctorModel<TI> updatedDoctor)
    {
        throw new NotImplementedException();
    }

    protected override DynamicParameters FieldsAsParams(DoctorEntityModel<TI, TC> entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        var p = new DynamicParameters();
        p.Add("DOCTOR", entity.Doctor);
        p.Add("NOBMRES", entity.Nombres);
        p.Add("APELLIDO_PATERNO", entity.Apellido_paterno);
        p.Add("APELLIDO_MATERNO", entity.Apellido_materno);
        p.Add("FECHA_NACIMIENTO", entity.Fecha_nacimiento);
        p.Add("ESPECIALIDAD", entity.Especialidad);
        return p;
    }

    protected override DynamicParameters KeyAsParams(TI key)
    {
        var p = new DynamicParameters();
        p.Add("ID", key);
        return p;
    }
}
