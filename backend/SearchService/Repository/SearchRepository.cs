using Newtonsoft.Json;
using SearchService.DTO;

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
            var response = await _httpClient.GetAsync($"/api/Database?query={searchQuery}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Der var fisk i min kaffe");
            }
            
            var result = JsonConvert.DeserializeObject<SearchResultDto>(await response.Content.ReadAsStringAsync());
            return result;
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            throw new Exception("Error while fetching data from DB API");
        }
    }
}