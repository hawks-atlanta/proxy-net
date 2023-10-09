using Microsoft.AspNetCore.Mvc;
using proxy_net.Adapters;
using proxy_net.Handler;
using proxy_net.Models;
using proxy_net.Repositories.Share;
using ServiceReference;

namespace proxy_net.Controllers.Share
{
    [ApiController]
    [Route("share")]
    public class ShareListController : ControllerBase
    {
        private readonly ILogger<ShareListController> _logger;
        private readonly IShareRepository _shareRepository;

        public ShareListController(ILogger<ShareListController> logger, IShareRepository shareRepository)
        {
            _logger = logger;
            _shareRepository = shareRepository;
        }

        [HttpPost("list", Name = "Share_List")]
        public async Task<IActionResult> Post([FromBody] authorization authorization)
        {
            if (authorization == null || authorization.token == null)
            {
                return BadRequest(new ResponseError
                {
                    code = 400,
                    msg = "El cuerpo de la solicitud es nulo o incompleto",
                    error = true
                });
            }

            try
            {
                share_listResponse response = await _shareRepository.ShareList(authorization);
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
                return Ok(new { response.@return.sharedFiles, response.@return.msg });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en la llamada SOAP.");
                throw;
            }
        }
    }
}