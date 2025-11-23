using Application.DTOs;
using Application.Interfaces.IService;
using Microsoft.AspNetCore.Mvc;
using Presentacion.Models;

namespace Presentacion.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        // Dependency Injection for AuthService
        private readonly IAuthService _authService;

        // Constructor
        public UserController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequestDTO loginRequest)
        {
            // Validam 
            if (loginRequest == null ||
                string.IsNullOrWhiteSpace(loginRequest.UserName) ||
                string.IsNullOrWhiteSpace(loginRequest.Password))
            {
                var badRequestResponse = new ApiResponse<object>(
                    Message: "Debe ingresar nombre de usuario y contraseña.",
                    Results: null,
                    Confirmation: false
                );
                return BadRequest(badRequestResponse);
            }

            try
            {
                // 
                var authResult = await _authService.ObtenerUsuarioAutenticadoAsync(loginRequest);

                if (authResult == null || string.IsNullOrEmpty(authResult.Token))
                {
                    var unauthorizedResponse = new ApiResponse<object>(
                        Message: "Usuario o contraseña incorrectos.",
                        Results: null,
                        Confirmation: false
                    );
                    return Unauthorized(unauthorizedResponse);
                }

                var successResponse = new ApiResponse<object>(
                    Message: "Usuario autenticado correctamente.",
                    Results: authResult,
                    Confirmation: true
                );
                return Ok(successResponse);
            }
            catch (UnauthorizedAccessException ex)
            {
                var unauthorizedResponse = new ApiResponse<object>(
                    Message: ex.Message,
                    Results: null,
                    Confirmation: false
                );
                return Unauthorized(unauthorizedResponse);
            }
            catch (ArgumentException ex)
            {
                var badRequestResponse = new ApiResponse<object>(
                    Message: ex.Message,
                    Results: null,
                    Confirmation: false
                );
                return BadRequest(badRequestResponse);
            }
            catch (Exception ex)
            {
                var errorResponse = new ApiResponse<object>(
                    Message: "Ocurrió un error inesperado durante la autenticación.",
                    Results: ex.Message,
                    Confirmation: false
                );
                return StatusCode(500, errorResponse);
            }
        }

        [HttpPost("sign-up")]
        public async Task<IActionResult> CrearUsuario([FromBody] LoginRequestDTO loginRequest)
        {

            // Validam 
            if (loginRequest == null ||
                string.IsNullOrWhiteSpace(loginRequest.UserName) ||
                string.IsNullOrWhiteSpace(loginRequest.Password))
            {
                var badRequestResponse = new ApiResponse<object>(
                    Message: "Debe ingresar nombre de usuario y contraseña.",
                    Results: null,
                    Confirmation: false
                );
                return BadRequest(badRequestResponse);
            }

            try
            {
                var result = await _authService.CrearUsuarioAsync(loginRequest);

                var successResponse = new ApiResponse<object>(
                Message: "Usuario creado correctamente.",
                Results: result.Username,
                Confirmation: true
                );
                return Ok(successResponse);
            }
            catch (UnauthorizedAccessException ex)
            {
                var unauthorizedResponse = new ApiResponse<object>(
                    Message: ex.Message,
                    Results: null,
                    Confirmation: false
                );
                return Unauthorized(unauthorizedResponse);
            }
            catch (ArgumentException ex)
            {
                var badRequestResponse = new ApiResponse<object>(
                    Message: ex.Message,
                    Results: null,
                    Confirmation: false
                );
                return BadRequest(badRequestResponse);
            }
            catch (Exception ex)
            {
                var errorResponse = new ApiResponse<object>(
                    Message: "Ocurrió un error inesperado durante la autenticación.",
                    Results: ex.Message,
                    Confirmation: false
                );
                return StatusCode(500, errorResponse);
            }



        }
    }
}
