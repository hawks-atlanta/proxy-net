using Microsoft.AspNetCore.Mvc;
using proxy_net.Adapters;
using proxy_net.Handler;
using proxy_net.Models;
using proxy_net.Repositories;
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
                return BadRequest(new ResponseError
                {
                    code = 400,
                    msg = "El cuerpo de la solicitud es nulo o incompleto.",
                    error = true
                });
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
                    var adapter = new ResponseAdapter(() => new ResponseError
                    {
                        code = response.@return.code,
                        msg = response.@return.msg,
                        error = response.@return.error
                    });

                    return this.HandleResponseError<IResponse>(adapter);
                }
                return Ok(new { response.@return });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en la llamada SOAP.");
                throw;
            }
        }
    }
}