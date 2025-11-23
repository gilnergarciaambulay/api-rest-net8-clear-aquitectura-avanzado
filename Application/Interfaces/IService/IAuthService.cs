using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IService
{
    public interface IAuthService 
    {
        Task<AuthResponseDTO> ObtenerUsuarioAutenticadoAsync(LoginRequestDTO loginRequestDTO);
        Task<AuthResponseDTO>  CrearUsuarioAsync(LoginRequestDTO loginRequestDTO);
    }
}
