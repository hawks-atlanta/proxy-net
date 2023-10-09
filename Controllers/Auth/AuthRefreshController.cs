using Microsoft.AspNetCore.Mvc;
using proxy_net.Adapters;
using proxy_net.Handler;
using proxy_net.Models;
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
            if (authorization == null || string.IsNullOrEmpty(authorization.token))
            {
                return BadRequest(new ResponseError
                {
                    code = 400,
                    msg = "El cuerpo de la solicitud es nulo o incompleto",
                    error = true
                });
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
                    var adapter = new ResponseAdapter(() => new ResponseError
                    {
                        code = response.@return.code,
                        msg = response.@return.msg,
                        error = response.@return.error
                    });

                    return this.HandleResponseError<IResponse>(adapter);
                }
                return Ok(new { Token = response?.@return.auth.token });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en la llamada SOAP.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Un error ocurrió mientras se procesaba la solicitud.");
            }
        }
    }
}
