using code.business.impl;
using code.business.service;
using code.models;
using Microsoft.AspNetCore.Mvc;

namespace code.Controllers;

[ApiController]
[Route("[controller]")]
public class DoctoresController(IDoctorBusiness<int> doctorBusiness) : ControllerBase
{
    private readonly IDoctorBusiness<int> doctorBusiness = doctorBusiness;

    [HttpGet]
    public async Task<IEnumerable<DoctorOutputModel<int>>?> Get() => await doctorBusiness.GetDoctores().ConfigureAwait(false);

    [HttpPost]
    public async Task<DoctorModel<int>> Add(DoctorModel<int> doctor) => await doctorBusiness.AddDoctor(doctor).ConfigureAwait(false);
}