using Serilog;
using System.Diagnostics;

namespace Presentacion.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Serilog.ILogger _logger = Log.ForContext<RequestLoggingMiddleware>();

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var sw = Stopwatch.StartNew();
            var request = context.Request;

            _logger.Information("HTTP {Method} {Path} - Iniciando solicitud", request.Method, request.Path);

            await _next(context);

            sw.Stop();
            _logger.Information("HTTP {Method} {Path} finalizada con {StatusCode} en {Elapsed} ms",
                request.Method, request.Path, context.Response.StatusCode, sw.ElapsedMilliseconds);
        }
    }
}
