using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    public class DatabaseSettings
    {
        private readonly IConfiguration _configuration;
        public DatabaseSettings(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetSqlServerConnectionString()
        {
            //return _configuration.GetConnectionString("SqlServerConnection") 
            //    ?? throw new InvalidOperationException("Connection string 'SqlServerConnection' not found.");
            var connection = _configuration.GetConnectionString("SqlServerConnection");
            if (string.IsNullOrWhiteSpace(connection))
                throw new InvalidOperationException("No se encontró la cadena de conexión para SQL Server.");
            return connection;
        }
        //Opcion don sin metodo: public string ConnectionString => _configuration.GetConnectionString("SqlServerConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }
}
