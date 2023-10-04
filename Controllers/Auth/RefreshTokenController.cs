using Microsoft.AspNetCore.Mvc;
using proxy_net.Models.Auth.Entities;
using ServiceReference;
using System.Net;
using System.ServiceModel.Channels;
using System.ServiceModel;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authentication;
using proxy_net.Controllers.Adapters;

namespace proxy_net.Controllers.Auth
{
    [ApiController]
    [Route("[controller]")]
    public class RefreshTokenController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;

        public RefreshTokenController(ILogger<LoginController> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "RefreshToken")]
        public async Task<IActionResult> Post([FromBody] User token)
        {
            if (string.IsNullOrEmpty(token.Token))
            {
                return BadRequest("El cuerpo de la solicitud es nulo o incompleto.");
            }
            try
            {
                return Ok(new { token = token.Token });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en la llamada SOAP.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error en la llamada SOAP: " + ex.Message);
            }
        }
    }
}
