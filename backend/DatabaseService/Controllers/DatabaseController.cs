using DatabaseService.DTO;
using DatabaseService.Monitoring;
using DatabaseService.Service;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DatabaseController(IDatabaseService service) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SearchResultDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SearchResultDto>> Get([FromQuery] string query)
    {
        using var activity = MonitoringService.ActivitySource.StartActivity("DatabaseController.Get");
        try
        {
            MonitoringService.Log.Information("Get request received");
            return Ok(await service.GetSearch(query));
        }
        catch (Exception e)
        {
            MonitoringService.Log.Error("Error in Get request: {0}", e.Message);
            Console.WriteLine(e);
            return NotFound(e.Message);
        }
    }
    
    [HttpPost("insert")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> InsertFiles([FromBody] ListScrubbedFilesDto files)
    {
        try
        {
            MonitoringService.Log.Information("Insert request received");
            await service.InsertFiles(files);
            return Ok();
        }
        catch (Exception e)
        {
            MonitoringService.Log.Error("Error in Insert request: {0}", e.Message);
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }
}