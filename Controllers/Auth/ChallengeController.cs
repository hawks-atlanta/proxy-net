using Microsoft.AspNetCore.Mvc;
using proxy_net.Models.Auth.Entities;
using proxy_net.Models.Auth.Repositories;

namespace proxy_net.Controllers.Auth
{
    [ApiController]
    [Route("[controller]")]
    public class ChallengeController : ControllerBase
    {
        private readonly IAuthRepository _userRepository;
        private readonly ILogger<LoginController> _logger;

        public ChallengeController(ILogger<LoginController> logger, IAuthRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        [HttpPost(Name = "challenge")]
        public async Task<IActionResult> Post([FromHeader(Name = "Authorization")] string jwtToken)
        {
            if (jwtToken == null)
            {
                return BadRequest("El cuerpo de la solicitud es nulo.");
            }
            jwtToken = jwtToken.Replace("Bearer ", "");
            var authenticatedUser = await _userRepository.PostChallengeAsync(jwtToken);
            if (authenticatedUser == null)
            {
                return Unauthorized();
            }
            return Ok(authenticatedUser);
        }       
    }
}
