﻿using Microsoft.AspNetCore.Mvc;
using proxy_net.Repositories.File;
using ServiceReference;

namespace proxy_net.Controllers.Account
{
    [ApiController]
    [Route("file")]
    public class FileDownloadController : ControllerBase
    {
        private readonly ILogger<FileDownloadController> _logger;
        private readonly IFileRepository _fileRepository;

        public FileDownloadController(ILogger<FileDownloadController> logger, IFileRepository fileRepository)
        {
            _logger = logger;
            _fileRepository = fileRepository;
        }

        [HttpPost("download", Name = "File_Download")]
        public async Task<IActionResult> Post([FromBody] reqFile reqFile)
        {
            if (string.IsNullOrEmpty(reqFile.fileUUID) || string.IsNullOrEmpty(reqFile.token))
            {
                return BadRequest("El cuerpo de la solicitud es nulo o incompleto.");
            }

            try
            {
                file_downloadResponse response = await _fileRepository.FileDownloadAsync(reqFile);

                if (response?.@return == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "La respuesta del servicio SOAP es nula o inválida.");
                }
                if (response.@return.error)
                {
                    string errorMessage = response.@return.msg;
                    return StatusCode(StatusCodes.Status401Unauthorized, errorMessage);
                }

                //`fileContent` es el byte[] que contiene los datos del archivo.
                byte[] fileContent = response.@return.fileContent;

                // Codificar el contenido del archivo a Base64
                string fileContentBase64;
                try
                {
                    fileContentBase64 = Convert.ToBase64String(fileContent);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al codificar el archivo.");
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error al codificar el archivo.");
                }
                return Ok(new { FileContent = fileContentBase64, FileName = response.@return.fileName });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en la llamada SOAP.");
                throw;
            }
        }
    }
}