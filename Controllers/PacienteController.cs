using code.business.impl;
using code.business.service;
using code.models;
using Microsoft.AspNetCore.Mvc;

namespace code.Controllers;

[ApiController]
[Route("[controller]")]
public class PacientesController(IPacienteBusiness<int> pacienteBusiness) : ControllerBase
{
    private readonly IPacienteBusiness<int> pacienteBusiness = pacienteBusiness;

    [HttpGet]
    public async Task<IEnumerable<PacienteModel<int>>?> Get() => await pacienteBusiness.GetPacientes().ConfigureAwait(false);

    [HttpPost]
    public async Task<PacienteModel<int>> Add(PacienteModel<int> paciente) => await pacienteBusiness.AddPaciente(paciente).ConfigureAwait(false);
}