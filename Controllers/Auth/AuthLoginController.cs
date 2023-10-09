using Microsoft.AspNetCore.Mvc;
using proxy_net.Adapters;
using proxy_net.Handler;
using proxy_net.Models;
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
        public async Task<IActionResult> Post([FromBody] credentials credentials)
        {
            if (string.IsNullOrEmpty(credentials.username) || string.IsNullOrEmpty(credentials.password))
            {
                return BadRequest("El cuerpo de la solicitud es nulo o incompleto.");
            }

            try
            {
                auth_loginResponse response = await _authRepository.LoginAsync(credentials);
                if (response?.@return == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "La respuesta del servicio SOAP es nula o inválida.");
                }
                if (response.@return.error)
                {
                    var adapter = new ResponseAdapter(() => new ResponseError
                    {
                        code = response.@return.code,
                        msg = response.@return.msg,
                        error = response.@return.error
                    });

                    return this.HandleResponseError<IResponse>(adapter);
                }
                return Created(string.Empty, new { response.@return.auth.token });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en la llamada SOAP.");
                throw;
            }
        }
    }
}
