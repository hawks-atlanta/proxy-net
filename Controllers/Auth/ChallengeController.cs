using Microsoft.AspNetCore.Mvc;
using proxy_net.Models.Auth.Entities;
using ServiceReference;
using System.Net;
using System.ServiceModel.Channels;
using System.ServiceModel;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authentication;

namespace proxy_net.Controllers.Auth
{
    [ApiController]
    [Route("[controller]")]
    public class ChallengeController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;

        public ChallengeController(ILogger<LoginController> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "challenge")]
        public async Task<object> Post()
        {
            string authorizationHeader = Request.Headers["Authorization"]!;
            if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
            {
                return AuthenticateResult.Fail("El cuerpo de la solicitud no es valido");
            }
            var token = authorizationHeader.Substring("Bearer ".Length);
            var authorization = new authorization
            {
                token = token,
            };
            Console.WriteLine("token: " + token);
            var httpRequestProperty = new HttpRequestMessageProperty();
            httpRequestProperty.Headers[HttpRequestHeader.Authorization] = authorizationHeader;
            await using(var client = new ServiceClient())
            {
                try
                {
                    using (new OperationContextScope(client.InnerChannel))
                    {
                        OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
                        var response = await client.auth_refreshAsync(authorization);
                        if (response == null || response.@return == null)
                    {
                            return StatusCode(StatusCodes.Status404NotFound, "La respuesta del servicio SOAP es nula o la propiedad 'return' es nula.");
                        }
                        return StatusCode(StatusCodes.Status200OK, response);
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error en la llamada SOAP: " + ex.Message);
                }
            }
        }
    }
}
