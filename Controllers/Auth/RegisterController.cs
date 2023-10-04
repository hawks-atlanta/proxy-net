using Microsoft.AspNetCore.Mvc;
using proxy_net.Controllers.Adapters;
using proxy_net.Models.Auth.Entities;
using ServiceReference;

namespace proxy_net.Controllers.Auth
{
    [ApiController]
    [Route("[controller]")]
    public class RegisterController : ControllerBase
    {
        private readonly ILogger<RegisterController> _logger;

        public RegisterController(ILogger<RegisterController> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "register")]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            try
            {
                if (string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Password))
                {
                    return BadRequest("El cuerpo de la solicitud es nulo o incompleto.");
                }

                var credentials = AdaptersToSoap.ConvertToCredentials(user);

                using (var client = new ServiceClient())
                {
                    account_registerResponse response = await client.account_registerAsync(credentials);
                    if (response?.@return != null)
                    {
                        if (response.@return.error)
                        {
                            string errorMessage = response.@return.msg;
                            if (errorMessage.Contains("duplicate key value violates unique constraint"))
                            {
                                return StatusCode(StatusCodes.Status409Conflict, new { ErrorSoap = errorMessage, error = "El usuario ya existe"});
                            }
                            return StatusCode(StatusCodes.Status401Unauthorized, errorMessage);
                        }
                        else if (!string.IsNullOrEmpty(response.@return.auth?.token))
                        {
                            return Created(string.Empty, new { Token = response.@return.auth.token });
                        }
                    }
                    return NotFound("No se pudo crear el usuario o la respuesta del servicio SOAP es inválida.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en la llamada SOAP.");
                throw;
            }
        }
    }
}