using Microsoft.AspNetCore.Mvc;
using proxy_net.Models.Auth.Entities;
using proxy_net.Repositories;
using ServiceReference;

namespace proxy_net.Controllers.Auth
{
    [ApiController]
    [Route("auth")]
    public class AuthLoginController : ControllerBase
    {
        private readonly ILogger<AuthLoginController> _logger;
        private readonly IAuthRepository _authRepository;

        public AuthLoginController(ILogger<AuthLoginController> logger, IAuthRepository authRepository)
        {
            _logger = logger;
            _authRepository = authRepository;
        }

        [HttpPost("login",Name = "Auth_Login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(auth_loginResponse))]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            if (string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Password))
            {
                return BadRequest("El cuerpo de la solicitud es nulo o incompleto.");
            }

            try
            {
                auth_loginResponse response = await _authRepository.LoginAsync(user);

                if (response?.@return == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "La respuesta del servicio SOAP es nula o inválida.");
                }
                if (response.@return.error)
                {
                    string errorMessage = response.@return.msg;
                    return StatusCode(StatusCodes.Status401Unauthorized, errorMessage);
                }
                string authToken = response?.@return.auth?.token ?? string.Empty;
                if (string.IsNullOrEmpty(authToken))
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Token de autenticación no válido.");
                }

                return Ok(new { Token = authToken });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en la llamada SOAP.");
                throw;
            }
        }
    }
}
