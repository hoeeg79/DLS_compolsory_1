using Microsoft.AspNetCore.Mvc;
using SearchService.DTO;
using SearchService.Entities;
using SearchService.Repository;

namespace SearchService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SearchController : ControllerBase
{
    private readonly ISearchRepository _searchRepository;
    
    public SearchController(ISearchRepository searchRepository)
    {
        _searchRepository = searchRepository;
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SearchResultDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SearchResultDto>> GetSearch([FromQuery] string searchQuery)
    {
        try
        {
            return Ok( await _searchRepository.GetSearch(searchQuery));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return NotFound(e.Message);
        }
    }
}