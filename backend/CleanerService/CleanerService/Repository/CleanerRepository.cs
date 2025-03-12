using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
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
        JsonSerializerOptions options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        var json = JsonSerializer.Serialize(new { Files = files }, options);

        using StringContent jsonContent = new(json, Encoding.UTF8, "application/json");

        await _httpClient.PostAsync("/api/database/insert", jsonContent);
    }
}