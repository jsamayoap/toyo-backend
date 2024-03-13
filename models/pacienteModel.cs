namespace code.models;

public sealed record PacienteModel<TI>(TI Paciente, string Nombres, string Apellido_paterno, string Apellido_materno, DateTime Fecha_nacimiento, int Nacionalidad)
    :Persona(Nombres, Apellido_paterno, Apellido_materno, Fecha_nacimiento)
where TI : struct, IEquatable<TI>
{
    public PacienteModel() : this(default, "", "", "", DateTime.Now, 0)
    {
    }

    public PacienteModel(TI id, PacienteModel<TI> data) : this(id, data.Nombres, data.Apellido_paterno, data.Apellido_materno, data.Fecha_nacimiento, data.Nacionalidad)
    {
    }
}