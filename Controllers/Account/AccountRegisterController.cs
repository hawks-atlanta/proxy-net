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
    public class AccountRegisterController : ControllerBase
    {
        private readonly ILogger<AccountRegisterController> _logger;
        private readonly IAccountRepository _accountRepository;

        public AccountRegisterController(ILogger<AccountRegisterController> logger, IAccountRepository accountRepository)
        {
            _logger = logger;
            _accountRepository = accountRepository;
        }

        [HttpPost("register", Name = "Account_Register")]
        public async Task<IActionResult> Post([FromBody] credentials credentials)
        {
            if (string.IsNullOrEmpty(credentials.username) || string.IsNullOrEmpty(credentials.password))
            {
                return BadRequest("El cuerpo de la solicitud es nulo o incompleto.");
            }
            try
            {
                account_registerResponse response = await _accountRepository.AccountRegisterAsync(credentials);
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
                return Created(string.Empty, new { response.@return.auth.token });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en la llamada SOAP.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Un error ocurrió mientras se procesaba la solicitud.");
            }
        }
    }
}