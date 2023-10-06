using Microsoft.AspNetCore.Mvc;
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
                if (response?.@return != null)
                {
                    if (response.@return.error)
                    {
                        string errorMessage = response.@return.msg;
                        if (errorMessage.Contains("duplicate key value violates unique constraint"))
                        {
                            return StatusCode(StatusCodes.Status409Conflict, new { ErrorSoap = errorMessage, error = "El usuario ya existe" });
                        }
                        return StatusCode(StatusCodes.Status401Unauthorized, errorMessage);
                    }
                    else if (!string.IsNullOrEmpty(response.@return.auth?.token))
                    {
                        return Created(string.Empty, new { response.@return.auth.token });
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en la llamada SOAP.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Un error ocurrió mientras se procesaba la solicitud.");
            }
            return NotFound("No se pudo crear el usuario o la respuesta del servicio SOAP es inválida.");
        }
    }
}