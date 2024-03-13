using System.Diagnostics.CodeAnalysis;

namespace code.common;

public sealed class ConnectionString
{
    public ConnectionString()
    {
        RelationalDBConn = string.Empty;
    }

    [SetsRequiredMembers]
    public ConnectionString(string relationalDBConn)
    {
        RelationalDBConn = relationalDBConn;
    }

    /// <summary>
    /// Cadena de conexión a base de datos relacional
    /// </summary>
    public required string RelationalDBConn { get; set; }
} 