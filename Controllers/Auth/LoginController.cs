﻿using Microsoft.AspNetCore.Mvc;
using proxy_net.Controllers.Adapters;
using proxy_net.Models.Auth.Entities;
using ServiceReference;

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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en la llamada SOAP.");
                throw;
            }
        }
    }
}
