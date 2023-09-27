using Microsoft.AspNetCore.Mvc;
using proxy_net.Models.Auth.Entities;
using ServiceReference;

namespace proxy_net.Controllers.Auth
{
    [ApiController]
    [Route("[controller]")]
    public class RegisterController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;

        public RegisterController(ILogger<LoginController> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "register")]
        public async Task<object> Post([FromBody] User user)
        {
            if (string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Password))
            {
                return BadRequest("El cuerpo de la solicitud es nulo.");
            }

            var credentials = new credentials
            {
                username = user.Username,
                password = user.Password
            };
            await using (var client = new ServiceClient())
            {
                try
                {
                    var response = await client.account_registerAsync(credentials);
                    if (response == null || response.@return == null)
                    {
                        return StatusCode(StatusCodes.Status404NotFound, "La respuesta del servicio SOAP es nula.");
                    }
                    return StatusCode(StatusCodes.Status201Created, response);
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error en la llamada SOAP: " + ex.Message);
                }
            }
        }
    }
}