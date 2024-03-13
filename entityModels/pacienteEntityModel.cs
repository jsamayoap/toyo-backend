using code.common;
using code.models;

namespace code.entityModels;

public sealed class PacienteEntityModel<TI, TC> : RelationalEntity<TI, TC>
        where TC : struct
        where TI : struct, IEquatable<TI>
{
    public TI Paciente { get; set; }
    public string? Nombres { get; set; }
    public string? Apellido_paterno { get; set; }
    public string? Apellido_materno { get; set; }
    public DateTime Fecha_nacimiento { get; set; }
    public int Nacionalidad { get; set; }

    public override TI Key { get => Paciente; set => Paciente = value; }

    public static explicit operator PacienteModel<TI>(PacienteEntityModel<TI, TC> x)
    {
        return new PacienteModel<TI>(x.Paciente, x.Nombres ?? "", x.Apellido_paterno ?? "", x.Apellido_materno ?? "", x.Fecha_nacimiento, x.Nacionalidad);
    }
}