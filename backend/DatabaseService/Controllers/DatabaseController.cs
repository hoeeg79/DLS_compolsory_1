using DatabaseService.DTO;
using DatabaseService.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DatabaseController : ControllerBase
{
    private readonly DatabaseRepository _repository;
    
    public DatabaseController(DatabaseRepository repository)
    {
        _repository = repository;
    }
    
    [HttpGet("{query}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SearchResultDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get([FromQuery] string query)
    {
        try
        {
            return Ok( await _repository.GetSearch(query));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return NotFound(e.Message);
        }
    }
}