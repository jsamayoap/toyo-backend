using code.business.service;
using code.models;
using code.repositories.services;

namespace code.business.impl;

public sealed class PacienteBusiness<TI, TC>(IPacienteRepository<TI, TC> pacienteRepository) : IPacienteBusiness<TI> 
    where TI : struct, IEquatable<TI>
    where TC : struct
{
    internal readonly IPacienteRepository<TI, TC> pacienteRepository = pacienteRepository;

    public async Task<PacienteModel<TI>> AddPaciente(PacienteModel<TI> paciente)
    {
        var x = await pacienteRepository.AddPaciente(paciente).ConfigureAwait(false);
        return new PacienteModel<TI>(x, paciente);
    }

    public async Task<IEnumerable<PacienteModel<TI>>?> GetPacientes()
    {
        var x = await pacienteRepository.GetPaciente().ConfigureAwait(false);
        return x?.Select(x => (PacienteModel<TI>)x);
    }
}