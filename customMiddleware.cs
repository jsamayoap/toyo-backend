using System.Net;
using System.Text.Json;
using code.Interfaces;
using Serilog;

namespace code;

/// <summary>
/// Constructor del middleware
/// </summary>
/// <param name="next"></param>
public class CustomMiddleware<TC>(RequestDelegate next)
        where TC : struct
{
    private readonly RequestDelegate next = next;

    /// <summary>
    /// Customización del middleware de .NetCore para inyectar un modelo de "unit of work"
    /// </summary>
    /// <param name="context"></param>
    /// <param name="rkm"></param>
    /// <returns></returns>
    public async Task Invoke(HttpContext context, IRelationalContext<TC> rkm)
    {
        try
        {
            if (rkm != null)
            {
                rkm.GetTransactionContext();
                await next(context).ConfigureAwait(false);
                rkm.CommitTransactionContext();
            }
            else
            {
                throw new ArgumentNullException(nameof(rkm));
            }
        }
        catch (ApplicationException ex)
        {
            if (rkm != null) rkm.RollbackTransaction(ex);
            if (context != null) await HandleExceptionAsync(context, ex).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            if (rkm != null)
            {
                rkm.RollbackTransaction(ex);
            }
            if (context != null) await HandleExceptionAsync(context, ex).ConfigureAwait(false);
        }
        finally
        {
            if (rkm != null)
            {
                rkm.CloseTransactionContext();
            }
        }
    }

    /// <summary>
    /// Manejo de expeción para no enviar al frontend un stack de error del backend
    /// </summary>
    /// <param name="context"></param>
    /// <param name="ex"></param>
    /// <returns>Información amigable y segura que puede procesar el frontend y por ende el usuario final</returns>
    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var code = HttpStatusCode.InternalServerError; // 500 if unexpected
        var result = string.Empty;
        if (ex is ApplicationException)
        {
            code = HttpStatusCode.BadRequest;
            result = JsonSerializer.Serialize(new ErrorMessage(ex.Message));
        }
        else if (ex is Exception)
        {
            Log.Logger.Error(ex, "Unexpected internal error");
            result = JsonSerializer.Serialize(new ErrorMessage("Something unexpectedly bad has occurred, we are going to dig into this"));
        }
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;
        return context.Response.WriteAsync(result);
    }
}