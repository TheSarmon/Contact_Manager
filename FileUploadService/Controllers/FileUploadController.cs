using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using FileUploadService.Services;
using FileUploadService.Dtos;

namespace FileUploadService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileUploadController : ControllerBase
    {
        private readonly IFileProcessingService _fileProcessingService;

        public FileUploadController(IFileProcessingService fileProcessingService)
        {
            _fileProcessingService = fileProcessingService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadCsv(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file provided.");
            }

            var result = await _fileProcessingService.ProcessFileAsync(file);

            return Ok("File processed successfully.");
        }
    }
}
