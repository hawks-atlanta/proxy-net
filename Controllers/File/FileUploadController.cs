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
            if (string.IsNullOrEmpty(fileContentBase64) || string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(token))
            {
                return BadRequest("Uno o más campos requeridos no se proporcionaron.");
            }

            byte[] fileContent = Base64Converter.DecodeBase64(fileContentBase64);

            //DTO para el request (encapsular y transportar datos)
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
                if (string.Equals(response?.@return.msg, "unauthorized", StringComparison.OrdinalIgnoreCase))
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, new { msg = "unauthorized", fileUUID = (string?)null, error = true });
                }
                else if (response?.@return.error == true || response?.@return.fileUUID == null)
                {
                    _logger.LogError("Error from the SOAP service: {0}", new { response?.@return.msg, response?.@return.fileUUID, response?.@return.error });
                    return StatusCode(StatusCodes.Status500InternalServerError, new { response?.@return.msg, response?.@return.fileUUID, response?.@return.error });
                }
                return Created(string.Empty, new { response?.@return.fileUUID });
            }
            else
            {
                _logger.LogError("Invalid response from the SOAP service");
                return StatusCode(StatusCodes.Status500InternalServerError, "Invalid response from the SOAP service");
            }
        }

        public class Base64Converter
        {
            public static byte[] DecodeBase64(string base64)
            {
                try
                {
                    return Convert.FromBase64String(base64);
                }
                catch (FormatException ex)
                {
                    throw new ArgumentException("Invalid Base64 format", ex);
                }
            }
        }
    }

}
