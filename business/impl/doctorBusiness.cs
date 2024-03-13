using code.business.service;
using code.models;
using code.repositories.services;

namespace code.business.impl;

public sealed class DoctorBusiness<TI, TC>(IDoctorRepository<TI, TC> doctorRepository) : IDoctorBusiness<TI> 
    where TI : struct, IEquatable<TI>
    where TC : struct
{
    internal readonly IDoctorRepository<TI, TC> doctorRepository = doctorRepository;

    public async Task<DoctorModel<TI>> AddDoctor(DoctorModel<TI> doctor)
    {
        var x = await doctorRepository.AddDoctor(doctor).ConfigureAwait(false);
        return new DoctorModel<TI>(x, doctor);
    }

    public async Task<IEnumerable<DoctorModel<TI>>?> GetDoctores()
    {
        var x = await doctorRepository.GetDoctor().ConfigureAwait(false);
        return x?.Select(x => (DoctorModel<TI>)x);
    }
}