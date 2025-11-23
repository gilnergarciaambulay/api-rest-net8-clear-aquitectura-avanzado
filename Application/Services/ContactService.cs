using Application.DTOs;
using Application.Interfaces.IRepository;
using Application.Interfaces.IService;
//using Microsoft.VisualBasic;
using Serilog;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;


namespace Application.Services
{
    public class ContactService: IContactService
    {
        private readonly IContactRepository _contactRepository;
        private readonly ILogger _logger = Log.ForContext<ContactService>();

        public ContactService(IContactRepository contactRepository) 
        { 
            _contactRepository = contactRepository;
        }

        public async Task<IEnumerable<ContactDTO>> ObtenerTodosContactosAsync()
        {
            _logger.Information("Caso de uso: ObtenerTodosContactosAsync iniciado");

            var contacts = await _contactRepository.GetAllContactsAsync();

            if (!contacts.Any())
            {
                _logger.Warning("No se encontraron contactos registrados");
            }

            var contactDTOs = contacts.Select(c => new ContactDTO
            {
                Id = c.Id,
                Nombre = c.Nombre,
                Celular = c.Celular,
                Telefono = c.Telefono,
                Email = c.Email
            }).ToList();

            _logger.Information("Caso de uso completado ({Count} contactos)", contactDTOs.Count);

            return contactDTOs;
        }

        public async Task<ContactDTO> ObtenerContactosPorIdAsync(int contactId)
        {
            _logger.Information("Caso de uso: ObtenerContactosPorIdAsync iniciado para ContactId: {ContactId}", contactId);

            var contact = await _contactRepository.GetContactByIdAsync(contactId);

            if (contact == null)
            {
                _logger.Warning("No se encontró contacto con Id: {ContactId}", contactId);
            }

            var contactDTO = new ContactDTO
            {
                Id = contact.Id,
                Nombre = contact.Nombre,
                Celular = contact.Celular,
                Telefono = contact.Telefono,
                Email = contact.Email
            };

            _logger.Information("Caso de uso completado para ContactId: {ContactId}", contactId);

            return contactDTO;
        }

        public async Task<bool> CrearContactoAsync(ContactDTO contactDto)
        {
            _logger.Information("Caso de uso: CrearContactoAsync iniciado para Nombre: {Nombre}", contactDto.Nombre);

            // Llamar al repositorio para agregar el contacto
           bool result =  await _contactRepository.AddContactAsync(contactDto);


            if (result)
            {
                _logger.Information("Caso de uso completado para Nombre: {Nombre}", contactDto.Nombre);
                return true;

            }

            return false;
        }

        public async Task<ContactDTO> ActualizarContactoAsync(ContactDTO contactDto)
        {
            // Mapear el DTO a la entidad Contact
            var contact = new Core.Entities.Contact
            {
                Id = contactDto.Id,
                Nombre = contactDto.Nombre,
                Celular = contactDto.Celular,
                Telefono = contactDto.Telefono,
                Email = contactDto.Email
            };
            // Llamar al repositorio para actualizar el contacto
            await _contactRepository.UpdateContactAsync(contactDto);
            // Retornar el DTO con los datos del contacto actualizado
            var contactDTO = new ContactDTO
            {
                Id = contact.Id,
                Nombre = contact.Nombre,
                Celular = contact.Celular,
                Telefono = contact.Telefono,
                Email = contact.Email
            };
            return contactDTO;
        }

        public Task EliminarContactoAsync(int contactId)
        {
            //try
            //{
            //var eliminado = 
            _contactRepository.DeleteContactAsync(contactId);
            return Task.CompletedTask;

            //return eliminado; // true si se eliminó, false si no existía
            //}
            //catch (Exception ex)
            //{
            //    // Aquí puedes registrar el error (por ejemplo, con un logger)
            //    Console.WriteLine($"Error eliminando el contacto: {ex.Message}");
            //    //return false;
            //}
        }

        public async Task<IEnumerable<ContactDTO>> BuscarContactosPorNombreAsync(string nombre)
        {
            var contacts = await _contactRepository.SearchByNameAsync(nombre);

            if (contacts == null)
                return null;

            var contactDTOs = contacts.Select(c => new ContactDTO
            {
                Id = c.Id,
                Nombre = c.Nombre,
                Celular = c.Celular,
                Telefono = c.Telefono,
                Email = c.Email
            }).ToList();
            return contactDTOs;
        }
    }
}
