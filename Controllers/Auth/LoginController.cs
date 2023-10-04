using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using proxy_net.Controllers.Adapters;
using proxy_net.Models.Auth.Entities;
using ServiceReference;
using System;
using System.ServiceModel;
using System.Threading.Tasks;

namespace proxy_net.Controllers.Auth
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;

        public LoginController(ILogger<LoginController> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "login")]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            if (string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Password))
            {
                return BadRequest("El cuerpo de la solicitud es nulo o incompleto.");
            }

            var credentials = AdaptersToSoap.ConvertToCredentials(user);

            try
            {
                using (var client = new ServiceClient())
                {
                    client.InnerChannel.OperationTimeout = TimeSpan.FromSeconds(50);
                    auth_loginResponse response = await client.auth_loginAsync(credentials);

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
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "InvalidOperationException(Operación no válida en la llamada SOAP)");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error en la llamada SOAP: " + ex.Message);

            }
            catch (TimeoutException ex)
            {
                _logger.LogError(ex, "TimeoutException(Error en la llamada SOAP.)");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error en la llamada SOAP: " + ex.Message);

            }
            catch (FaultException<MissingFieldException> ex)
            {
                _logger.LogError(ex, "FaultException(MissingFieldException | Library has been removed or renamed)");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error en la llamada SOAP: " + ex.Message);
            }
            catch (FaultException ex)
            {
                _logger.LogError(ex, "FaultException(Error en la llamada SOAP.)");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error en la llamada SOAP: " + ex.Message);
            }
            catch (CommunicationException ex)
            {
                _logger.LogError(ex, "CommunicationException(Error en la llamada SOAP.)");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error en la llamada SOAP: " + ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en la llamada SOAP.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error en la llamada SOAP: " + ex.Message);
            }
        }
    }
}
