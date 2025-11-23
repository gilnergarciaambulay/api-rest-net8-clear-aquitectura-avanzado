using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Application.Interfaces.IService
{
    public interface IContactService
    {

        Task<IEnumerable<ContactDTO>> ObtenerTodosContactosAsync();
        Task<ContactDTO> ObtenerContactosPorIdAsync(int contactId);
        Task<bool> CrearContactoAsync(ContactDTO contactDto);
        Task<ContactDTO> ActualizarContactoAsync(ContactDTO contactDto);
        Task EliminarContactoAsync(int contactId);
        Task<IEnumerable<ContactDTO>> BuscarContactosPorNombreAsync(string nombre);
    }
}
