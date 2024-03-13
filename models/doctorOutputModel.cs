namespace code.models;

public sealed record DoctorOutputModel<TI>(TI Doctor, string Nombres, string Apellido_paterno, string Apellido_materno, DateTime Fecha_nacimiento, string Especialidad)
    :Persona(Nombres, Apellido_paterno, Apellido_materno, Fecha_nacimiento)
where TI : struct, IEquatable<TI>
{
    public DoctorOutputModel() : this(default, string.Empty, string.Empty, string.Empty, DateTime.Now, string.Empty)
    {
    }
}