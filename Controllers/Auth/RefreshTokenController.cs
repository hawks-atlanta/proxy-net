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
        public async Task<IActionResult> Post()
        {
            string authorizationHeader = Request.Headers["Authorization"]!;
            if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
            {
                return BadRequest("El cuerpo de la solicitud no es valido");
            }

            var token = authorizationHeader.Substring("Bearer ".Length);
            var authorization = AdaptersToSoap.ConvertToAuthorization(token);
            Console.WriteLine("Token: " + token);
            Console.WriteLine("Adapter: " + authorization);

            var httpRequestProperty = new HttpRequestMessageProperty();
            httpRequestProperty.Headers[HttpRequestHeader.Authorization] = authorizationHeader;
            await using(var client = new ServiceClient())
            {
                try
                {
                    using (new OperationContextScope(client.InnerChannel))
                    {
                        OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
                        auth_refreshResponse response = await client.auth_refreshAsync(authorization);
                        Console.WriteLine("auth_refreshResponse: " + response);
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
