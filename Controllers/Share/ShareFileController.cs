﻿using Microsoft.AspNetCore.Mvc;
using proxy_net.Adapters;
using proxy_net.Handler;
using proxy_net.Models;
using proxy_net.Repositories.Share;
using ServiceReference;

namespace proxy_net.Controllers.Share
{
    [ApiController]
    [Route("share")]
    public class ShareFileController : ControllerBase
    {
        private readonly ILogger<ShareFileController> _logger;
        private readonly IShareRepository _shareRepository;

        public ShareFileController(ILogger<ShareFileController> logger, IShareRepository shareRepository)
        {
            _logger = logger;
            _shareRepository = shareRepository;
        }

        [HttpPost("file", Name = "Share_File")]
        public async Task<IActionResult> Post([FromBody] reqShareFile reqShareFile)
        {
            if (reqShareFile == null)
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
                share_fileResponse response = await _shareRepository.ShareFile(reqShareFile);
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
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en la llamada SOAP.");
                throw;
            }
        }
    }
}