using code.common;
using code.entityModels;
using code.interfaces;
using code.Interfaces;
using code.models;
using code.repositories.services;
using Dapper;

namespace code.repositories.impl;

public sealed class PacienteRepository<TI, TC> : DataAccess<PacienteEntityModel<TI, TC>, TI, TC>, IPacienteRepository<TI, TC>
        where TI : struct, IEquatable<TI>
        where TC : struct
{
    public PacienteRepository(IRelationalContext<TC> rkm,
                            ISQLData queries,
                            ILogger<PacienteRepository<TI, TC>> logger) : base(rkm, queries, logger)
    {
    }

    public async Task<TI> AddPaciente(PacienteModel<TI> doctor)
    {
        var p = new DynamicParameters();
        p.Add("NOMBRES", doctor.Nombres);
        p.Add("APELLIDO_PATERNO", doctor.Apellido_paterno);
        p.Add("APELLIDO_MATERNO", doctor.Apellido_materno);
        p.Add("FECHA_NACIMIENTO", doctor.Fecha_nacimiento);
        p.Add("NACIONALIDAD", doctor.Nacionalidad);
        return await Add<TI>(p).ConfigureAwait(false);
    }

    public Task DeletePaciente(TI id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<PacienteEntityModel<TI, TC>>> GetPaciente(TI doctorID)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<PacienteEntityModel<TI, TC>>?> GetPaciente()
    {
        return await GetALL(null).ConfigureAwait(false);
    }

    public Task UpdatePaciente(PacienteModel<TI> updatedDoctor)
    {
        throw new NotImplementedException();
    }

    protected override DynamicParameters FieldsAsParams(PacienteEntityModel<TI, TC> entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        var p = new DynamicParameters();
        p.Add("PACIENTE", entity.Paciente);
        p.Add("NOBMRES", entity.Nombres);
        p.Add("APELLIDO_PATERNO", entity.Apellido_paterno);
        p.Add("APELLIDO_MATERNO", entity.Apellido_materno);
        p.Add("FECHA_NACIMIENTO", entity.Fecha_nacimiento);
        p.Add("NACIONALIDAD", entity.Nacionalidad);
        return p;
    }

    protected override DynamicParameters KeyAsParams(TI key)
    {
        var p = new DynamicParameters();
        p.Add("ID", key);
        return p;
    }
}
