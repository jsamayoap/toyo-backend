namespace code;

public sealed class ErrorMessage(string error)
{

    /// <summary>
    /// Descripción de error
    /// </summary>
    public string? Error { get; set; } = error;
}