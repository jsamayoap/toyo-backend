using code.models;

namespace code.business.service;

public interface IDoctorBusiness<TI>
 where TI : struct, IEquatable<TI>
{
    Task<DoctorModel<TI>> AddDoctor(DoctorModel<TI> doctor);
    Task<IEnumerable<DoctorOutputModel<TI>>?> GetDoctores();
}