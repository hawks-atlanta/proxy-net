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
                return BadRequest(new ResponseError
                {
                    code = 400,
                    msg = "El cuerpo de la solicitud es nulo o incompleto",
                    error = true
                });
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
