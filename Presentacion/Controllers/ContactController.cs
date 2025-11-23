using Application.DTOs;
using Application.Interfaces.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentacion.Models;

namespace Presentacion.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }
        [HttpGet("contact-all")]
        public async Task<IActionResult> GetAllContactsAsync()
        {
            var contact = await _contactService.ObtenerTodosContactosAsync();
            var response = new ApiResponse<IEnumerable<object>>(
                Message: "Contacts retrieved successfully",
                Results: contact,
                Confirmation: true
                );
            return Ok(response);
        }

        [HttpGet("contact-id")]
        public async Task<IActionResult> GetContactByIdAsync(int id)
        {
            var contact = await _contactService.ObtenerContactosPorIdAsync(id);
            if (contact == null)
            {
                var notFoundResponse = new ApiResponse<object>(
                    Message: "Contact not found",
                    Results: null,
                    Confirmation: false
                    );
                return NotFound(notFoundResponse);
            }
            var response = new ApiResponse<object>(
                Message: "Contact retrieved successfully",
                Results: contact,
                Confirmation: true
                );
            return Ok(response);
        }

        [Authorize]
        [HttpPost("contact-create")]
        public async Task<IActionResult> CreateContactAsync([FromBody] ContactDTO contactDto)
        {
            try
            {
                var createdContact = await _contactService.CrearContactoAsync(contactDto);
                var response = new ApiResponse<object>(
                    Message: "Contact created successfully",
                    Results: createdContact,
                    Confirmation: true
                    );
                return Ok(response);
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

        [Authorize]
        [HttpPut("contact-update")]
        public async Task<IActionResult> UpdateContactAsync([FromBody] ContactDTO contactDto)
        {
            var updatedContact = await _contactService.ActualizarContactoAsync(contactDto);
            if (updatedContact == null)
            {
                var notFoundResponse = new ApiResponse<object>(
                    Message: "Contact not found",
                    Results: null,
                    Confirmation: false
                    );
                return NotFound(notFoundResponse);
            }
            var response = new ApiResponse<object>(
                Message: "Contact updated successfully",
                Results: updatedContact,
                Confirmation: true
                );
            return Ok(response);
        }

        [Authorize]
        [HttpDelete("contact-delete")]
        public async Task<IActionResult> DeleteContactAsync(int id)
        {
            try
            {
               await _contactService.EliminarContactoAsync(id);
                //if (!deleted)
                //{
                //    var notFoundResponse = new ApiResponse<object>(
                //        Message: "Contact not found",
                //        Results: null,
                //        Confirmation: deleted
                //        );
                //    return NotFound(notFoundResponse);
                //}
                var response = new ApiResponse<object>(
                    Message: "Contact deleted successfully",
                    Results: id,
                    Confirmation: true
                    );
                return Ok(response);
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

        [HttpGet("contact-like-name")]
        public async Task<IActionResult> SearchContactsByNameAsync(string nombre)
        {
            var contacts = await _contactService.BuscarContactosPorNombreAsync(nombre);
            var response = new ApiResponse<IEnumerable<object>>(
                Message: "Contacts retrieved successfully",
                Results: contacts,
                Confirmation: true
                );
            return Ok(response);
        }
    }
}
