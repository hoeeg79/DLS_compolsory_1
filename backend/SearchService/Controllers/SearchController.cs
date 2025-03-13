using Microsoft.AspNetCore.Mvc;
using SearchService.DTO;
using SearchService.Entities;
using SearchService.Monitoring;
using SearchService.Repository;

namespace SearchService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SearchController(ISearchRepository searchRepository) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SearchResultDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SearchResultDto>> GetSearch([FromQuery] string searchQuery)
    {
        try
        {
            MonitoringService.Log.Information("Starting new search. Search query: {searchQuery}", 
                searchQuery);
            using var activity = MonitoringService.ActivitySource.StartActivity();
            return Ok( await searchRepository.GetSearch(searchQuery));
        }
        catch (Exception e)
        {
            MonitoringService.Log.Error(e, "Error during search. Search query: {searchQuery}", 
                searchQuery);
            Console.WriteLine(e);
            return NotFound(e.Message);
        }
    }
}