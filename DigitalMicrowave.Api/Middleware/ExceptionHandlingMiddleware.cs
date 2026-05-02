using System.Net;
using System.Text.Json;
using DigitalMicrowave.Business.Exceptions;

namespace DigitalMicrowave.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (MicrowaveBusinessException ex)
        {
            await WriteError(context, HttpStatusCode.BadRequest, ex.Message, ex.Field);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro não tratado: {Message} | Inner: {Inner} | Stack: {Stack}",
                ex.Message, ex.InnerException?.Message, ex.StackTrace);

            await WriteError(context, HttpStatusCode.InternalServerError, "Ocorreu um erro interno. Tente novamente.");
        }
    }

    private static Task WriteError(HttpContext context, HttpStatusCode status, string message, string? field = null)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)status;
        return context.Response.WriteAsync(JsonSerializer.Serialize(new { success = false, message, field }));
    }
}
