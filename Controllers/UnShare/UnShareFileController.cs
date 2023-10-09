using Microsoft.AspNetCore.Mvc;
using proxy_net.Adapters;
using proxy_net.Handler;
using proxy_net.Models;
using proxy_net.Repositories.Share;
using proxy_net.Repositories.UnShare;
using ServiceReference;

namespace proxy_net.Controllers.Share
{
    [ApiController]
    [Route("unshare")]
    public class UnShareFileController : ControllerBase
    {
        private readonly ILogger<ShareFileController> _logger;
        private readonly IUnShareRepository _unshareRepository;

        public UnShareFileController(ILogger<ShareFileController> logger, IUnShareRepository unshareRepository)
        {
            _logger = logger;
            _unshareRepository = unshareRepository;
        }

        [HttpPost("file", Name = "UnShare_File")]
        public async Task<IActionResult> Post([FromBody] reqShareRemove reqShareRemove)
        {
            if (reqShareRemove == null)
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
                unshare_fileResponse response = await _unshareRepository.UnShareFile(reqShareRemove);
                //TODO: no enviar por body la respuesta
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
                return Ok( new { response.@return.msg });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en la llamada SOAP.");
                throw;
            }
        }
    }
}