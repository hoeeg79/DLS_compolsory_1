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

    public async Task<SearchResultDto?> GetSearch(SearchDto searchQuery)
    {
        try
        {
            var response = await _httpClient.GetAsync($"http://localhost:5000/api/db?query={searchQuery.Query}");
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