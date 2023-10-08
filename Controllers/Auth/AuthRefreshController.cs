using Microsoft.AspNetCore.Mvc;
using proxy_net.Repositories;
using proxy_net.Repositories.File;
using ServiceReference;

namespace proxy_net.Controllers.Auth
{
    [ApiController]
    [Route("auth")]
    public class AuthRefreshController : ControllerBase
    {
        private readonly ILogger<AuthLoginController> _logger;
        private readonly IAuthRepository _authRepository;

        public AuthRefreshController(ILogger<AuthLoginController> logger, IAuthRepository authRepository)
        {
            _logger = logger;
            _authRepository = authRepository;
        }

        [HttpPost("refresh", Name = "Auth_Refresh")]
        //When using NEW refreshToken/Challenge change to:
        //TODO:
        //public async Task<IActionResult> Post([FromBody] User token)
        public async Task<IActionResult> Post([FromBody] authorization authorization)
        {
            if (authorization == null)
            {
                return BadRequest("El cuerpo de la solicitud es nulo o incompleto.");
            }
            try
            {
                auth_refreshResponse response = await _authRepository.RefreshAsync(authorization);

                if (response?.@return == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "La respuesta del servicio SOAP es nula o inválida.");
                }
                if (response.@return.error)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, new { response.@return.msg, response.@return.error, response.@return.code });
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
                return StatusCode(StatusCodes.Status500InternalServerError, "Un error ocurrió mientras se procesaba la solicitud.");
            }
        }
    }
}
