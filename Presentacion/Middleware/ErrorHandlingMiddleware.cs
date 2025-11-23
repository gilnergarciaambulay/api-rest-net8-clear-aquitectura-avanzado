using Serilog;
using System.Net;
using System.Text.Json;

namespace Presentacion.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // Log estructurado con Serilog
                Log.Error(ex, "Error procesando solicitud {@Path}", context.Request.Path);

                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = HttpStatusCode.InternalServerError;
            string message = "Ocurrió un error interno en el servidor.";

            // Puedes personalizar distintos códigos según tipo de error
            if (exception is ArgumentException) statusCode = HttpStatusCode.BadRequest;
            if (exception is KeyNotFoundException) statusCode = HttpStatusCode.NotFound;

            var response = new
            {
                Message = message,
                Error = exception.Message,
                Path = context.Request.Path,
                Timestamp = DateTime.UtcNow
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
        }
    }
}
