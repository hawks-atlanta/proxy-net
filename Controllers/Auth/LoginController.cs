﻿using Microsoft.AspNetCore.Mvc;
using proxy_net.Controllers.SimulatedServices;
using proxy_net.Models.Auth.DataSources;
using proxy_net.Models.Auth.Entities;
using proxy_net.Models.Auth.Repositories;

namespace proxy_net.Controllers.Auth
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IAuthRepository _userRepository;
        private readonly ILogger<LoginController> _logger;

        public LoginController(ILogger<LoginController> logger, IAuthRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        [HttpPost(Name = "login")]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest("El cuerpo de la solicitud es nulo.");
            }
            Console.WriteLine($"Username: {user.Username}, Password: {user.Password}, JwtToken: {user.JwtToken}");
            var authenticatedUser = await _userRepository.PostLoginAsync(user.Username, user.Password);
            if (authenticatedUser == null)
            {
                return Unauthorized();
            }
            return Ok(authenticatedUser);
        }
    }
}