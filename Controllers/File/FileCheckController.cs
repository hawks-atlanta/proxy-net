using Microsoft.AspNetCore.Mvc;
using proxy_net.Repositories.File;
using ServiceReference;

namespace proxy_net.Controllers.Account
{
    [ApiController]
    [Route("file")]
    public class FileCheckController : ControllerBase
    {
        private readonly ILogger<FileCheckController> _logger;
        private readonly IFileRepository _fileRepository;

        public FileCheckController(ILogger<FileCheckController> logger, IFileRepository fileRepository)
        {
            _logger = logger;
            _fileRepository = fileRepository;
        }

        [HttpPost("check", Name = "File_Check")]
        public async Task<IActionResult> Post([FromBody] reqFile reqFile)
        {
            if (string.IsNullOrEmpty(reqFile.fileUUID) || string.IsNullOrEmpty(reqFile.token))
            {
                return BadRequest("El cuerpo de la solicitud es nulo o incompleto.");
            }
            try
            {
                file_checkResponse response = await _fileRepository.FileCheckAsync(reqFile);

                if (response?.@return == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "La respuesta del servicio SOAP es nula o inválida.");
                }
                if (response.@return.error)
                {
                    string errorMessage = response.@return.msg;
                    return StatusCode(StatusCodes.Status401Unauthorized, errorMessage);
                }
                return Ok(new { response.@return.ready });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en la llamada SOAP.");
                throw;
            }
        }
    }
}