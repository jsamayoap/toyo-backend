namespace code.models;

public sealed record DoctorModel<TI>(TI Doctor, string Nombres, string Apellido_paterno, string Apellido_materno, DateTime Fecha_nacimiento, int Especialidad)
    :Persona(Nombres, Apellido_paterno, Apellido_materno, Fecha_nacimiento)
where TI : struct, IEquatable<TI>
{
    public DoctorModel() : this(default, "", "", "", DateTime.Now, 0)
    {
    }

    public DoctorModel(TI id, DoctorModel<TI> data) : this(id, data.Nombres, data.Apellido_paterno, data.Apellido_materno, data.Fecha_nacimiento, data.Especialidad)
    {
    }
}