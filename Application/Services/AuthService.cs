using Application.DTOs;
using Application.Interfaces.IRepository;
using Application.Interfaces.IService;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _usuarioRepository;
        private readonly IConfiguration _config;

        public AuthService(IUserRepository usuarioRepository, IConfiguration configuration)
        {
            _usuarioRepository = usuarioRepository;
            _config = configuration;
        }

        public async Task<AuthResponseDTO> CrearUsuarioAsync(LoginRequestDTO loginRequestDTO)
        {
            // Validar parámetros
            if (string.IsNullOrWhiteSpace(loginRequestDTO.UserName) || string.IsNullOrWhiteSpace(loginRequestDTO.Password))
                throw new ArgumentException("El usuario y la contraseña son obligatorios.");

           var result= await _usuarioRepository.AddUserAsync(loginRequestDTO);

            return new AuthResponseDTO
            {
                Username = result.NombreUsuario
            };


        }
        public async Task<AuthResponseDTO> ObtenerUsuarioAutenticadoAsync(LoginRequestDTO loginRequestDTO)
        {
            // Validar parámetros
            if (string.IsNullOrWhiteSpace(loginRequestDTO.UserName) || string.IsNullOrWhiteSpace(loginRequestDTO.Password))
                throw new ArgumentException("El usuario y la contraseña son obligatorios.");

            // Consultar repositorio
            var user = await _usuarioRepository.GetAuthenticateUserAsync(loginRequestDTO);
    

            if (user == null)
                throw new UnauthorizedAccessException("Usuario o contraseña incorrectos.");

            // Generar token JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]!);
            var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            // Aqui hace la seguridad
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UsuarioID.ToString()),
                    new Claim(ClaimTypes.Name, user.NombreUsuario),
                    new Claim(ClaimTypes.Email, user.CorreoElectronico ?? "")
                }),
                Expires = DateTime.UtcNow.AddMinutes(
                    double.Parse(_config["Jwt:ExpireMinutes"]!)
                ),
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            // Aqui devuelveme  
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new AuthResponseDTO
            {
                Username = user.NombreUsuario,
                Token = tokenHandler.WriteToken(token)
            };
        }
    }
}
