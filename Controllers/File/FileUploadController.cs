using Microsoft.AspNetCore.Mvc;
using proxy_net.Repositories.File;
using ServiceReference;

namespace proxy_net.Controllers.File
{
    [ApiController]
    [Route("file")]
    public class FileUploadController : ControllerBase
    {
        private readonly ILogger<FileUploadController> _logger;
        private readonly IFileRepository _fileRepository;

        public FileUploadController(ILogger<FileUploadController> logger, IFileRepository fileRepository)
        {
            _logger = logger;
            _fileRepository = fileRepository;
        }

        [HttpPost("upload", Name = "File_Upload")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(file_uploadResponse))]
        public async Task<IActionResult> Post([FromForm] string fileContentBase64, [FromForm] string fileName, [FromForm] string token)
        {
            if (string.IsNullOrEmpty(fileContentBase64) || string.IsNullOrEmpty(fileName))
            {
                return BadRequest("File or file name is not provided");
            }

            byte[] fileContent;
            try
            {
                fileContent = Convert.FromBase64String(fileContentBase64);
            }
            catch (FormatException ex)
            {
                _logger.LogError(ex, "Error al decodificar el archivo.");
                return BadRequest("File is not properly base64 encoded");
            }
            var reqFileUpload = new reqFileUpload
            {
                fileName = fileName,
                fileContent = fileContent,
                location = null,
                token = token
            };
            try
            {
                file_uploadResponse response = await _fileRepository.FileUploadAsync(reqFileUpload);
                return HandleFileUploadResponse(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en la llamada SOAP.");
                throw;
            }
        }
        private IActionResult HandleFileUploadResponse(file_uploadResponse response)
        {
            if (response?.@return != null)
            {
                return Created(string.Empty, new { response.@return.fileUUID });
            }
            else
            {
                if (response?.@return.error == true)
                {
                    _logger.LogError("Error from the SOAP service: {0}", response.@return.msg);
                    return StatusCode(StatusCodes.Status500InternalServerError, response.@return.msg);
                }
                _logger.LogError("Invalid response from the SOAP service");
                return StatusCode(StatusCodes.Status500InternalServerError, "Invalid response from the SOAP service");
            }
        }
    }

}
