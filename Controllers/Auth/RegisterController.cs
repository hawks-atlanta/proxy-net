using Microsoft.AspNetCore.Mvc;
using proxy_net.Adapters;
using proxy_net.Models.Auth.Entities;
using proxy_net.Repositories;
using ServiceReference;

namespace proxy_net.Controllers.Auth
{
    [ApiController]
    [Route("account")]
    public class RegisterController : ControllerBase
    {
        private readonly ILogger<RegisterController> _logger;
        private readonly IAuthRepository _authRepository;

        public RegisterController(ILogger<RegisterController> logger, IAuthRepository authRepository)
        {
            _logger = logger;
            _authRepository = authRepository;
        }

        [HttpPost("register",Name = "Account_Register")]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            if (string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Password))
            {
                return BadRequest("El cuerpo de la solicitud es nulo o incompleto.");
            }
            try
            {
                account_registerResponse response = await _authRepository.RegisterAsync(user);
                if (response?.@return != null)
                {
                    if (response.@return.error)
                    {
                        string errorMessage = response.@return.msg;
                        if (errorMessage.Contains("duplicate key value violates unique constraint"))
                        {
                            return StatusCode(StatusCodes.Status409Conflict, new { ErrorSoap = errorMessage, error = "El usuario ya existe" });
                        }
                        return StatusCode(StatusCodes.Status401Unauthorized, errorMessage);
                    }
                    else if (!string.IsNullOrEmpty(response.@return.auth?.token))
                    {
                        return Created(string.Empty, new { Token = response.@return.auth.token });
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en la llamada SOAP.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Un error ocurrió mientras se procesaba la solicitud.");
            }
            return NotFound("No se pudo crear el usuario o la respuesta del servicio SOAP es inválida.");
        }
    }
}