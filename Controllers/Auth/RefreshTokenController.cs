﻿using Microsoft.AspNetCore.Mvc;
using proxy_net.Models.Auth.Entities;

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
        //When using NEW refreshToken/Challenge change to:
        //TODO:
        //public async Task<IActionResult> Post([FromBody] User token)
        public IActionResult Post([FromBody] User token)
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
                throw;
            }
        }
    }
}
