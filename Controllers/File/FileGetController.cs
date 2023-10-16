using Microsoft.AspNetCore.Mvc;
using proxy_net.Adapters;
using proxy_net.Handler;
using proxy_net.Models;
using proxy_net.Repositories.File;
using ServiceReference;

namespace proxy_net.Controllers.File
{
    [ApiController]
    [Route("file")]
    public class FileGetControler : ControllerBase
    {
        private readonly ILogger<FileGetControler> _logger;
        private readonly IFileRepository _fileRepository;

        public FileGetControler(ILogger<FileGetControler> logger, IFileRepository fileRepository)
        {
            _logger = logger;
            _fileRepository = fileRepository;
        }

        [HttpPost("get", Name = "File_Get")]
        public async Task<IActionResult> Post([FromBody] reqFile request)
        {
            if (request == null || string.IsNullOrEmpty(request.token) || string.IsNullOrEmpty(request.fileUUID))
            {
                return BadRequest(new ResponseError
                {
                    code = 400,
                    msg = "El cuerpo de la solicitud es nulo o incompleto",
                    error = true
                });
            }

            var reqFile = new reqFile
            {
                fileUUID = request.fileUUID,
                token = request.token
            };

            try
            {
                file_getResponse response = await _fileRepository.FileGetAsync(reqFile);
                if (response?.@return == null)
                {
                    return NotFound(new ResponseError
                    {
                        code = 404,
                        msg = "La respuesta del servicio SOAP es nula o inválida.",
                        error = true
                    });
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
                return Ok(new { response.@return.file, response.@return.msg });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en la llamada SOAP.");
                throw;
            }
        }
    }
}