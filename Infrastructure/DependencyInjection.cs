using Application.DTOs;
using Application.Interfaces.IRepository;
using Infrastructure.Persistence;
using Infrastructure.Persistence.SqlServer;
using Infrastructure.Persistence.SqlServer.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Registrar DatabaseSettings
            services.AddSingleton<DatabaseSettings>();

            // Registrar la fábrica de conexiones usando la cadena de conexión del config
            services.AddScoped(provider =>
            {
                //var logger = provider.GetRequiredService<SqlServerConnectionFactory>();
                var config = provider.GetRequiredService<DatabaseSettings>();
                var connectionString = config.GetSqlServerConnectionString();
                //var connectionString = config.ConnectionString;
                return new SqlServerConnectionFactory(connectionString);
            });

            // Registrar el repositorio
            services.AddScoped<IContactRepository, ContactRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            return services;
        }
    }
}
