using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using CleanerService.Models;
using Polly;
using Polly.CircuitBreaker;

namespace CleanerService.Repository;

public class CleanerRepository : ICleanerRepository
{
    private readonly HttpClient _httpClient;
    private AsyncCircuitBreakerPolicy _circuitBreakerPolicy;
    

    public CleanerRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _circuitBreakerPolicy = Policy.Handle<Exception>()
            .CircuitBreakerAsync(3, TimeSpan.FromSeconds(30),
                (ex, ts) =>
                {
                    Console.WriteLine($"Circuit broken: sleep {ts.TotalSeconds} seconds");
                },
                () =>
                {
                    Console.WriteLine("Circuit reset");
                });
    }

    public async Task SendCleanFiles(List<CleanedFileDto> files)
    {
        await _circuitBreakerPolicy.ExecuteAsync(async () =>
        {
            JsonSerializerOptions options = new()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            var json = JsonSerializer.Serialize(new { Files = files }, options);

            using StringContent jsonContent = new(json, Encoding.UTF8, "application/json");

            await _httpClient.PostAsync("/api/database/insert", jsonContent);
        });
    }
}