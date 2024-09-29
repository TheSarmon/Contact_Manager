using CsvHelper;
using FileUploadService.Dtos;
using FileUploadService.Entities;
using FileUploadService.Mappers;
using System.Globalization;

namespace FileUploadService.Services
{
    public interface IFileProcessingService
    {
        Task<ProcessResult> ProcessFileAsync(IFormFile file);
    }

    public class FileProcessingService : IFileProcessingService
    {
        private readonly HttpClient _httpClient; // Для відправки даних у DataService

        public FileProcessingService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ProcessResult> ProcessFileAsync(IFormFile file)
        {
            var contacts = new List<ContactDto>();

            using var reader = new StreamReader(file.OpenReadStream());
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var entities = csv.GetRecords<ContactEntity>().ToList();
            
            contacts = entities.Select(ContactMapper.ToDto).ToList();

            if (contacts == null || !contacts.Any())
            {
                return new ProcessResult { IsSuccess = false, Errors = new List<string> { "No contacts found in the file." } };
            }

            var response = await _httpClient.PostAsJsonAsync("http://localhost:5001/api/contacts/bulk", contacts);

            if (response.IsSuccessStatusCode)
            {
                return new ProcessResult { IsSuccess = true };
            }
            else
            {
                return new ProcessResult
                {
                    IsSuccess = false,
                    Errors = new List<string> { "Failed to save contacts in DataService." }
                };
            }
        }
    }

    public class ProcessResult
    {
        public bool IsSuccess { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}
