using Application;
using Infrastructure;
using Infrastructure.Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Presentacion.Middleware;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// -------------------------
// Configurar Serilog
// -------------------------
builder.Host.AddSerilogLogging(builder.Configuration);

// -------------------------
// Configurar JWT
// -------------------------
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
        ),
        ClockSkew = TimeSpan.Zero // sin margen extra al expirar
    };
});

builder.Services.AddAuthorization();

// -------------------------
// Configurar servicios
// -------------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// -------------------------
// Inyeccion de dependencias 
// -------------------------
builder.Services.AddApplication(); // Desde Application
builder.Services.AddInfrastructure(builder.Configuration); // Desde Infrastructure

var app = builder.Build();

// -------------------------
// Middleware
// -------------------------

// Captura global de errores (al inicio)
app.UseMiddleware<ErrorHandlingMiddleware>();

// Logging de requests
app.UseMiddleware<RequestLoggingMiddleware>();

// Swagger solo en desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Autenticación y autorización (orden importante)
app.UseAuthentication();
app.UseAuthorization();

// Mapear controladores
app.MapControllers();

//app.Run();

// -------------------------
// Ejecución con logging
// -------------------------
try
{
    Log.Information("Iniciando MyApi...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "La aplicación falló al iniciarse correctamente");
}
finally
{
    Log.CloseAndFlush();
}

