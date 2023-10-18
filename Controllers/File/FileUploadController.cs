using Microsoft.AspNetCore.Mvc;
using proxy_net.Adapters;
using proxy_net.Handler;
using proxy_net.Models;
using proxy_net.Repositories.File;
using ServiceReference;

namespace proxy_net.Controllers.File
{
    public class FileUploadRequest
    {
        public string FileName { get; set; } = null!;
        public List<string> FileContent { get; set; } = null!;
        public string? Location { get; set; } = null;
        public string Token { get; set; } = null!;
    }

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
        public async Task<IActionResult> Post([FromBody] FileUploadRequest request)
        {
            if (string.IsNullOrEmpty(request.FileName) ||
                string.IsNullOrEmpty(request.Token) ||
                request.FileContent == null ||
                !request.FileContent.Any())
            {
                return BadRequest(new ResponseError
                {
                    code = 400,
                    msg = "El cuerpo de la solicitud es nulo o incompleto",
                    error = true
                });
            }

            List<byte> allFileContent = new List<byte>();

            try
            {
                foreach (var contentBase64 in request.FileContent)
                {
                    var fileContent = Convert.FromBase64String(contentBase64);
                    allFileContent.AddRange(fileContent);
                }
            }
            catch (FormatException)
            {
                return BadRequest(new ResponseError
                {
                    code = 400,
                    msg = "Invalid Base64 Format",
                    error = true
                });
            }

            var reqFileUpload = new reqFileUpload
            {
                fileName = request.FileName,
                fileContent = allFileContent.ToArray(),
                location = request.Location,
                token = request.Token
            };

            try
            {
                file_uploadResponse response = await _fileRepository.FileUploadAsync(reqFileUpload);
                if (response?.@return == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "La respuesta del servicio SOAP es nula o inválida.");
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
                return Created(string.Empty, new { response?.@return.fileUUID });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en la llamada SOAP.");
                throw;
            }
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
