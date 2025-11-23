using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

/*
// This static class provides an extension method to configure Serilog logging for an IHostBuilder.
dotnet add package Serilog
dotnet add package Serilog.Sinks.Console
dotnet add package Serilog.Sinks.File
dotnet add package Serilog.Settings.Configuration
dotnet add package Serilog.Enrichers.Environment
dotnet add package Serilog.Enrichers.Thread
dotnet add package Serilog.Extensions.Hosting

para Integrar Seq para ver logs en un Dashboard visua
dotnet add package Serilog.Sinks.Seq
 */

namespace Infrastructure.Logging
{
    public static class SerilogConfiguration
    {
        public static void AddSerilogLogging(this IHostBuilder hostBuilder, IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .Enrich.WithEnvironmentName()
                .Enrich.WithThreadId()
                .Enrich.WithMachineName()
                .WriteTo.Console()
                .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
                .WriteTo.Seq("http://localhost:5341") // 👈 Aquí agregamos el destino Seq
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Information()
                .CreateLogger();
            hostBuilder.UseSerilog(); // Ahora UseSerilog estará disponible
        }
    }
}
