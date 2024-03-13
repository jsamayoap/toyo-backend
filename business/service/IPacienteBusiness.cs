using code.models;

namespace code.business.service;

public interface IPacienteBusiness<TI>
 where TI : struct, IEquatable<TI>
{
    Task<PacienteModel<TI>> AddPaciente(PacienteModel<TI> persona);
    Task<IEnumerable<PacienteModel<TI>>?> GetPacientes();
}