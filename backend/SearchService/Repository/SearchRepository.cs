using Newtonsoft.Json;
using SearchService.DTO;
using SearchService.Monitoring;

namespace SearchService.Repository;

public class SearchRepository : ISearchRepository
{
    private readonly HttpClient _httpClient;

    public SearchRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<SearchResultDto?> GetSearch(string searchQuery)
    {
        try
        {
            MonitoringService.Log.Information("Entered SearchRepository.GetSearch with query: {searchQuery}",
                searchQuery);
            using var activity = MonitoringService.ActivitySource.StartActivity();
            var response = await _httpClient.GetAsync($"/api/Database?query={searchQuery}");
            if (!response.IsSuccessStatusCode)
            {
                MonitoringService.Log.Error("Error while fetching data from DB API. Status code: {statusCode}",
                    response.StatusCode);
                throw new Exception("Respons from DB-API was not successful");
            }
            
            var result = JsonConvert.DeserializeObject<SearchResultDto>(await response.Content.ReadAsStringAsync());
            return result;
        }
        catch (Exception exception)
        {
            MonitoringService.Log.Error(exception, "Error while fetching data from DB API");
            Console.WriteLine(exception);
            throw new Exception("Error while fetching data from DB API");
        }
    }
}