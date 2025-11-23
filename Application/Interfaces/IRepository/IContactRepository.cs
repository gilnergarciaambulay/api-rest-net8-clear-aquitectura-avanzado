using Application.DTOs;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IRepository
{
    public interface IContactRepository
    {
        Task<IEnumerable<Contact>> GetAllContactsAsync();
        Task<Contact> GetContactByIdAsync(int id);
        Task<bool> AddContactAsync(ContactDTO contactDTO);
        Task<Contact> UpdateContactAsync(ContactDTO contactDTO);
        Task DeleteContactAsync(int id);
        Task<IEnumerable<Contact>> SearchByNameAsync(string nombre);
    }
}
