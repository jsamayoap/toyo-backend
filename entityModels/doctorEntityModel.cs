using code.common;
using code.models;

namespace code.entityModels;

public sealed class DoctorEntityModel<TI, TC> : RelationalEntity<TI, TC>
        where TC : struct
        where TI : struct, IEquatable<TI>
{
    public TI Doctor { get; set; }
    public string? Nombres { get; set; }
    public string? Apellido_paterno { get; set; }
    public string? Apellido_materno { get; set; }
    public DateTime Fecha_nacimiento { get; set; }
    public int Especialidad { get; set; }

    public override TI Key { get => Doctor; set => Doctor = value; }

    public static explicit operator DoctorModel<TI>(DoctorEntityModel<TI, TC> x)
    {
        return new DoctorModel<TI>(x.Doctor, x.Nombres ?? "", x.Apellido_paterno ?? "", x.Apellido_materno ?? "", x.Fecha_nacimiento, x.Especialidad);
    }
}