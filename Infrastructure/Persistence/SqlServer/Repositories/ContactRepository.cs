using Application.DTOs;
using Application.Interfaces.IRepository;
using Core.Entities;
using Microsoft.Data.SqlClient;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Infrastructure.Persistence.SqlServer.Repositories
{
    public class ContactRepository : IContactRepository
    {
        // Dependency Injection
        private readonly SqlServerConnectionFactory _sqlServerConnectionFactory;
        private readonly ILogger _logger = Log.ForContext<ContactRepository>();

        // Constructor
        public ContactRepository(SqlServerConnectionFactory sqlServerConnectionFactory) 
        {
            _sqlServerConnectionFactory = sqlServerConnectionFactory;
        }

        public async Task<IEnumerable<Contact>> GetAllContactsAsync()
        {
            var contacts = new List<Contact>();

            try
            {
                await using var connection = await _sqlServerConnectionFactory.CreateConnectionAsync();

                _logger.Information("✅ Conexión abierta correctamente");

                await using var cmd = new SqlCommand("usp_Contacto_ListarTodo", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                // Medir tiempo de ejecución
                var sw = Stopwatch.StartNew();

                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    contacts.Add(new Contact
                    {
                        Id = (int)reader["id"],
                        Nombre = reader["nombre"]?.ToString() ?? string.Empty,
                        Celular = reader["celular"]?.ToString() ?? string.Empty,
                        Telefono = reader["telefono"]?.ToString() ?? string.Empty,
                        Email = reader["email"]?.ToString() ?? string.Empty,
                    });
                }

                await connection.CloseAsync();

                sw.Stop();

                _logger.Information("SP completado ({Count} registros en {Elapsed} ms)", contacts.Count, sw.ElapsedMilliseconds);


                return contacts;
            }
            catch (SqlException ex)
            {
                //Console.WriteLine($"❌ Error al obtener contactos: {ex.Message}");
                _logger.Error(ex, "Error al ejecutar SP sp_ListarContactos");
                throw;
            }


        }

        public async Task<Contact> GetContactByIdAsync(int id)
        {
            Contact contact = null;

            try
            {
                await using var connection = await _sqlServerConnectionFactory.CreateConnectionAsync();

                _logger.Information("✅ Conexión abierta correctamente");

                await using var cmd = new SqlCommand("usp_Contacto_ListarPorID", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@Id", id);

                var sw = new Stopwatch();

                await using var reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    contact = new Contact
                    {
                        Id = (int)reader["id"],
                        Nombre = reader["nombre"]?.ToString() ?? string.Empty,
                        Celular = reader["celular"]?.ToString() ?? string.Empty,
                        Telefono = reader["telefono"]?.ToString() ?? string.Empty,
                        Email = reader["email"]?.ToString() ?? string.Empty,
                    };
                }

                sw.Stop();

                _logger.Information("SP completado en {Elapsed} ms", sw.ElapsedMilliseconds);

                return contact;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error al ejecutar SP usp_Contacto_ListarPorID");
                throw;
            }
        }

        public async Task<bool> AddContactAsync(ContactDTO contactDTO)
        {
            try
            {
                await using var connection = await _sqlServerConnectionFactory.CreateConnectionAsync();
                _logger.Information("✅ Conexión abierta correctamente");
                await using var cmd = new SqlCommand("usp_Contacto_Insertar", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                // Agregar parámetros
                cmd.Parameters.AddWithValue("@nombre", contactDTO.Nombre);
                cmd.Parameters.AddWithValue("@celular", contactDTO.Celular);
                cmd.Parameters.AddWithValue("@telefono", contactDTO.Telefono);
                cmd.Parameters.AddWithValue("@email", contactDTO.Email);

                // Si el SP devuelve el Id insertado, puedes agregar un parámetro de salida:
                var outId = new SqlParameter("@OutID", System.Data.SqlDbType.Int) 
                { 
                    Direction = System.Data.ParameterDirection.Output 
                };
                cmd.Parameters.Add(outId);

                // Ejecutar el comando
                await cmd.ExecuteNonQueryAsync();

                if(outId.Value != DBNull.Value)
                {
                    int insertedId = (int)outId.Value;
                    _logger.Information("Contacto insertado con Id: {InsertedId}", insertedId);

                    return true;
                }

                return false;


            }
            catch (Exception ex)
            {
                //Console.WriteLine($"❌ Error al registrar contacto: {ex.Message}");
                _logger.Error(ex, "Error al ejecutar SP usp_Contacto_Insertar");
                throw;
            }

        }

        public async Task<Contact> UpdateContactAsync(ContactDTO contactDTO)
        {
            Contact? contact = null;

            try
            {
                // Crear y abrir la conexión
                await using var connection = await _sqlServerConnectionFactory.CreateConnectionAsync();

                _logger.Information("✅ Conexión abierta correctamente");

                await using var cmd = new SqlCommand("usp_Contacto_Actualizar", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                // Agregar parámetros
                cmd.Parameters.AddWithValue("@Id", contactDTO.Id);
                cmd.Parameters.AddWithValue("@Nombre", contactDTO.Nombre);
                cmd.Parameters.AddWithValue("@Celular", contactDTO.Celular);
                cmd.Parameters.AddWithValue("@Telefono", contactDTO.Telefono);
                cmd.Parameters.AddWithValue("@Email", contactDTO.Email);

                // Medir tiempo de ejecución
                var sw = Stopwatch.StartNew();

                await using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    contact = new Contact();
                    contact.Id = (int)reader["id"];
                    contact.Nombre = reader["Nombre"]?.ToString() ?? string.Empty;
                    contact.Celular = reader["Celular"]?.ToString() ?? string.Empty;
                    contact.Telefono = reader["Telefono"]?.ToString() ?? string.Empty;
                    contact.Email = reader["Email"]?.ToString() ?? string.Empty;
                }

                sw.Stop();
                _logger.Information($"✅ Procedimiento ejecutado en {sw.ElapsedMilliseconds} ms");

                return contact!;

            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error al ejecutar SP usp_Contacto_Actualizar");
                throw;
            }
        }

        public async Task DeleteContactAsync(int id)
        {
            try
            {
                await using var connection = await _sqlServerConnectionFactory.CreateConnectionAsync();

                _logger.Information("✅ Conexión abierta correctamente");

                await using var cmd = new SqlCommand("usp_Contacto_EliminarPorId", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                // Agregar parámetros
                cmd.Parameters.AddWithValue("@Id", id);

                // Medir tiempo de ejecución
                var sw = Stopwatch.StartNew();

                var result = await cmd.ExecuteScalarAsync();

                //if (Convert.ToInt32(result) == 0)
                //    return false;

                sw.Stop();

                _logger.Information("SP completado en {Elapsed} ms", sw.ElapsedMilliseconds);

                //return Convert.ToInt32(result) > 0; ;

            }
            catch (SqlException sqlEx)
            {
                // Captura el THROW del SP
                _logger.Warning("Error de SQL al eliminar contacto: {Message}", sqlEx.Message);
                throw new ApplicationException($"No se pudo eliminar: {sqlEx.Message}", sqlEx);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error inesperado al ejecutar SP usp_Contacto_EliminarPorId");
                throw;
            }

        }

        public async Task<IEnumerable<Contact>> SearchByNameAsync(string nombre)
        {
            var contacts = new List<Contact>();
            try
            {
                await using var connection = await _sqlServerConnectionFactory.CreateConnectionAsync();

                _logger.Information("✅ Conexión abierta correctamente");

                await using var cmd = new SqlCommand("usp_Contacto_ListaPorNombre", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@Nombre", nombre);

                var sw = Stopwatch.StartNew();

                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    contacts.Add(new Contact
                    {
                        Id = (int)reader["id"],
                        Nombre = reader["nombre"]?.ToString() ?? string.Empty,
                        Celular = reader["celular"]?.ToString() ?? string.Empty,
                        Telefono = reader["telefono"]?.ToString() ?? string.Empty,
                        Email = reader["email"]?.ToString() ?? string.Empty,
                    });
                }

                sw.Stop();

                _logger.Information("SP completado ({Count} registros en {Elapsed} ms)", contacts.Count, sw.ElapsedMilliseconds);

                return contacts;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al buscar contactos por nombre: {ex.Message}");
                throw;
            }
        }
    }
}
