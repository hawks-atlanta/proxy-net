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
    public class FileNewDirectoryController : ControllerBase
    {
        private readonly ILogger<FileNewDirectoryController> _logger;
        private readonly IFileRepository _fileRepository;

        public FileNewDirectoryController(ILogger<FileNewDirectoryController> logger, IFileRepository fileRepository)
        {
            _logger = logger;
            _fileRepository = fileRepository;
        }

        [HttpPost("new/directory", Name = "File_New_Directory")]
        public async Task<IActionResult> Post([FromBody] reqFileNewDir reqFileNewDir)
        {
            if (reqFileNewDir == null || string.IsNullOrEmpty(reqFileNewDir.directoryName) ||
                string.IsNullOrEmpty(reqFileNewDir.location) || string.IsNullOrEmpty(reqFileNewDir.token))
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
                file_new_dirResponse response = await _fileRepository.FileNewDirAsync(reqFileNewDir);
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
                return Ok(new { response.@return.fileUUID, response.@return.msg });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en la llamada SOAP.");
                throw;
            }
        }
    }
}