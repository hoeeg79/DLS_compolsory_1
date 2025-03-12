using System.Diagnostics;
using CleanerService.Service;
using Microsoft.AspNetCore.Mvc;
using Monitoring;
using OpenTelemetry.Trace;

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
        using (var activity = MonitoringService.ActivitySource.StartActivity("CleanFiles"))
        {

            if (files == null || files.Length == 0)
            {
                return BadRequest("No files uploaded.");
            }

            try
            {
                MonitoringService.Log.Information("Starting CleanerService with {files} files uploaded.", files.Length);
                await _cleanerService.CleanFiles(files);
                MonitoringService.TracerProvider.ForceFlush();
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal Server Error: {e.Message}");
            }
        }
    }
}