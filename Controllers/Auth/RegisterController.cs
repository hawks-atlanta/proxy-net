﻿using Microsoft.AspNetCore.Mvc;
using proxy_net.Models.Auth.Entities;
using proxy_net.Models.Auth.Repositories;

namespace proxy_net.Controllers.Auth
{
    [ApiController]
    [Route("[controller]")]
    public class RegisterController : ControllerBase
    {
        private readonly IAuthRepository _userRepository;
        private readonly ILogger<LoginController> _logger;

        public RegisterController(ILogger<LoginController> logger, IAuthRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        [HttpPost(Name = "register")]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            if (string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Password))
            {
                return BadRequest("El cuerpo de la solicitud es nulo.");
            }
            Console.WriteLine($"Username: {user.Username}, Password: {user.Password}, JwtToken: {user.JwtToken}");
            var jwtToken = await _userRepository.PostRegisterAsync(user.Username, user.Password);
            if (jwtToken.Jwt == null)
            {
                return Unauthorized("Unauthorized");
            }
            return StatusCode(StatusCodes.Status201Created, jwtToken);
        }
    }
}