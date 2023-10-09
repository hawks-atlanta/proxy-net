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
    public class ShareListWithWhoController : ControllerBase
    {
        private readonly ILogger<ShareListWithWhoController> _logger;
        private readonly IShareRepository _shareRepository;

        public ShareListWithWhoController(ILogger<ShareListWithWhoController> logger, IShareRepository shareRepository)
        {
            _logger = logger;
            _shareRepository = shareRepository;
        }

        [HttpPost("list/with/who", Name = "Share_List_With_Who")]
        public async Task<IActionResult> Post([FromBody] reqFile reqFile)
        {
            if (reqFile == null || string.IsNullOrEmpty(reqFile.token) || string.IsNullOrEmpty(reqFile.fileUUID))
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
                share_list_with_whoResponse response = await _shareRepository.ShareListWithWho(reqFile);
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
                return Ok(new { response.@return.usernames, response.@return.msg });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en la llamada SOAP.");
                throw;
            }
        }
    }
}