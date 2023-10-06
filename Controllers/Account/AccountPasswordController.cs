using Microsoft.AspNetCore.Mvc;
using proxy_net.Models.Auth.Entities;
using proxy_net.Repositories;
using proxy_net.Repositories.Account;
using ServiceReference;

namespace proxy_net.Controllers.Account
{
    [ApiController]
    [Route("account")]
    public class AccountPasswordController : ControllerBase
    {
        private readonly ILogger<AccountPasswordController> _logger;
        private readonly IAccountRepository _accountRepository;

        public AccountPasswordController(ILogger<AccountPasswordController> logger, IAccountRepository accountRepository)
        {
            _logger = logger;
            _accountRepository = accountRepository;
        }

        [HttpPost("password", Name = "Account_Password")]
        public async Task<IActionResult> Post([FromBody] reqAccPassword reqAccPassword)
        {
            if (string.IsNullOrEmpty(reqAccPassword.oldpassword) || string.IsNullOrEmpty(reqAccPassword.newpassword) || string.IsNullOrEmpty(reqAccPassword.token))
            {
                return BadRequest("El cuerpo de la solicitud es nulo o incompleto.");
            }
            try
            {
                account_passwordResponse response = await _accountRepository.AccountPasswordAsync(reqAccPassword);

                if (response?.@return == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "La respuesta del servicio SOAP es nula o inválida.");
                }
                if (response.@return.error)
                {
                    string errorMessage = response.@return.msg;
                    return StatusCode(StatusCodes.Status401Unauthorized, errorMessage);
                }
                //TODO: change response!
                string authToken = response?.@return.code.ToString() ?? string.Empty;
                if (string.IsNullOrEmpty(authToken))
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Token de autenticación no válido.");
                }
                //TODO: change response!
                return Ok(new { OK = response });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en la llamada SOAP.");
                throw;
            }
        }
    }
}