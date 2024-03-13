using code.entityModels;
using code.models;

namespace code.repositories.services;

public interface IDoctorRepository<TI, TC>
        where TI : struct, IEquatable<TI>
        where TC : struct
{
    Task<TI> AddDoctor(DoctorModel<TI> doctor);
    Task DeleteDoctor(TI id);
    Task<IEnumerable<DoctorEntityModel<TI, TC>>> GetDoctor(TI doctorID);
    Task<IEnumerable<DoctorEntityModel<TI, TC>>?> GetDoctor();
    Task<IEnumerable<DoctorOutputModel<TI>>?> GetDoctorCustom();
}