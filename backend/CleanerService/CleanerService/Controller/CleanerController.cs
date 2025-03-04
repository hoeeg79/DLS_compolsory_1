using CleanerService.Models;
using CleanerService.Service;
using Microsoft.AspNetCore.Mvc;

namespace CleanerService.Controller;

[Route("cleaner/")]
[ApiController]
public class CleanerController : ControllerBase
{
    private readonly ICleanerService _cleanerService;

    public CleanerController(ICleanerService cleanerService)
    {
        _cleanerService = cleanerService;
    }

    [HttpPost("clean")]
    public ActionResult CleanFiles([FromForm] IFormFile[]? files)
    {
        if (files == null || files.Length == 0)
        {
            return BadRequest("No files uploaded.");
        }

        try
        {
            var result = _cleanerService.CleanFiles(files);

            return Ok(result);
        }
        catch (Exception e)
        {
            return StatusCode(500, $"Internal Server Error: {e.Message}");
        }
    }
}