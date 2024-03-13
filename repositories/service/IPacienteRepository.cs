using code.entityModels;
using code.models;

namespace code.repositories.services;

public interface IPacienteRepository<TI, TC>
        where TI : struct, IEquatable<TI>
        where TC : struct
{
    Task<TI> AddPaciente(PacienteModel<TI> paciente);
    Task DeletePaciente(TI id);
    Task<IEnumerable<PacienteEntityModel<TI, TC>>> GetPaciente(TI pacienteID);
    Task<IEnumerable<PacienteEntityModel<TI, TC>>?> GetPaciente();
}