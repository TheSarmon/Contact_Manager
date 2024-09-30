using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Net.Http.Json;
using AutoMapper;
using CsvHelper;
using DataService.Domain;
using DataService.Entities;

namespace FileUploadService.Services
{
    public interface IFileProcessingService
    {
        Task<ProcessResult> ProcessFileAsync(IFormFile file);
    }

    public class FileProcessingService : IFileProcessingService
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;

        public FileProcessingService(HttpClient httpClient, IMapper mapper)
        {
            _httpClient = httpClient;
            _mapper = mapper;
        }

        public async Task<ProcessResult> ProcessFileAsync(IFormFile file)
        {
            using var reader = new StreamReader(file.OpenReadStream());
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            var entities = csv.GetRecords<ContactEntity>().ToList();

            var contacts = _mapper.Map<List<Contact>>(entities);

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
