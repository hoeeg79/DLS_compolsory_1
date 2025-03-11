using System.Text;
using System.Text.Json;
using CleanerService.Models;

namespace CleanerService.Repository;

public class CleanerRepository : ICleanerRepository
{
    private readonly HttpClient _httpClient;

    public CleanerRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task SendCleanFiles(List<CleanedFileDto> files)
    {
        
        using StringContent jsonContent = new(
            JsonSerializer.Serialize(new
                {
                    Files = files
                }
            ),
            Encoding.UTF8,
            "application/json"
        );

        _httpClient.BaseAddress = new Uri("http://db-api:8080");
        await _httpClient.PostAsync("/api/database/insert", jsonContent);
    }
}