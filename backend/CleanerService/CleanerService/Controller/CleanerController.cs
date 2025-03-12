using System.Text;
using System.Text.Json;
using CleanerService.Service;
using Microsoft.AspNetCore.Mvc;

namespace CleanerService.Controller;

[Route("/api/[controller]")]
[ApiController]
public class CleanerController : ControllerBase
{
    private readonly ICleanerService _cleanerService;

    public CleanerController(ICleanerService cleanerService)
    {
        _cleanerService = cleanerService;
    }

    [HttpPost("clean")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CleanFiles([FromForm] IFormFile[]? files)
    {
        if (files == null || files.Length == 0)
        {
            return BadRequest("No files uploaded.");
        }

        try
        {
            await _cleanerService.CleanFiles(files);

            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, $"Internal Server Error: {e.Message}");
        }
    }
}