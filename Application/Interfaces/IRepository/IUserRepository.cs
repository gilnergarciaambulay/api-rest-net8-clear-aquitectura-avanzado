using Application.DTOs;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IRepository
{
    public interface IUserRepository
    {
        Task<User> GetAuthenticateUserAsync(LoginRequestDTO loginRequestDTO);
        Task<User> AddUserAsync(LoginRequestDTO loginRequestDTO);
    }
}
