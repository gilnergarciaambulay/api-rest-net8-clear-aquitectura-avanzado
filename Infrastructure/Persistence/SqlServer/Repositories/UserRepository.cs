using Application.DTOs;
using Application.Interfaces.IRepository;
using Core.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.SqlServer.Repositories
{
    public class UserRepository : IUserRepository
    {   
        // Dependency Injection
        private readonly SqlServerConnectionFactory _sqlServerConnectionFactory;

        // Constructor
        public UserRepository(SqlServerConnectionFactory sqlServerConnectionFactory) 
        {
            _sqlServerConnectionFactory = sqlServerConnectionFactory;
        }
        
        public async Task<User> AddUserAsync(LoginRequestDTO loginRequestDTO)
        {
            if (loginRequestDTO is null)
                throw new ArgumentNullException(nameof(loginRequestDTO));

            if (string.IsNullOrWhiteSpace(loginRequestDTO.UserName))
                throw new ArgumentException("El nombre de usuario no puede estar vacío.", nameof(loginRequestDTO.UserName));

            if (string.IsNullOrWhiteSpace(loginRequestDTO.Password))
                throw new ArgumentException("La clave no puede estar vacía.", nameof(loginRequestDTO.Password));

            // Construir correo si no se proporciona en el username
            var correo = loginRequestDTO.UserName.Contains("@")
                ? loginRequestDTO.UserName
                : $"{loginRequestDTO.UserName}@gmail.com";

            try
            {
                await using var connection = await _sqlServerConnectionFactory.CreateConnectionAsync();

                await using var cmd = new SqlCommand("usp_Usuario_Insertar", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Usuario", loginRequestDTO.UserName);
                cmd.Parameters.AddWithValue("@Correo", correo);
                cmd.Parameters.AddWithValue("@Clave", loginRequestDTO.Password);
              
                await cmd.ExecuteNonQueryAsync();

                return new User
                {
                    NombreUsuario = loginRequestDTO.UserName,
                };
            }
            catch (SqlException sqlEx)
            {
                // Puedes mapear códigos específicos si el SP los devuelve para inserción
                //_logger.LogError($"Error de SQL: {sqlEx.Message}");
                throw new ApplicationException($"No se pudo agregar el usuario: {sqlEx.Message}", sqlEx);
            }

        }

        public async Task<User> GetAuthenticateUserAsync(LoginRequestDTO loginRequestDTO)
        {
            var user = new User();

            try
            {
                await using var connection = await _sqlServerConnectionFactory.CreateConnectionAsync();

                Console.WriteLine("✅ Conexión abierta correctamente");

                await using var cmd = new SqlCommand("usp_Usuario_ValidarInicioDeSesionPorUsuario", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@Usuario", loginRequestDTO.UserName);
                cmd.Parameters.AddWithValue("@Clave", loginRequestDTO.Password);

                using var reader = await cmd.ExecuteReaderAsync();

                if (!await reader.ReadAsync())
                    return null; // Usuario no autenticado

                user = new User
                {
                    UsuarioID = (int)reader["UsuarioID"],
                    NombreUsuario = reader["NombreUsuario"]?.ToString() ?? string.Empty,
                    CorreoElectronico = reader["CorreoElectronico"]?.ToString() ?? string.Empty
                };

                return user;
            }
            catch (SqlException ex)
            {
                //Console.WriteLine($"❌ Error al buscar usuario: {ex.Message}");
                // Manejo de errores personalizados del SP
                switch (ex.Number)
                {
                    case 50001: throw new Exception("Usuario vacío.");
                    case 50002: throw new Exception("Clave vacía.");
                    case 50003: throw new Exception("Usuario no encontrado.");
                    case 50004: throw new Exception("Clave incorrecta.");
                    default: throw;
                }
            }
        }
    }
}
