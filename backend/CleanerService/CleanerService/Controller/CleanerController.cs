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

    [HttpPost("/clean")]
    public ActionResult CleanFiles([FromForm] IFormFile[]? files)
    {
        if (files == null || files.Length == 0)
        {
            return BadRequest("No files uploaded.");
        }

        try
        {
            var result = _cleanerService.CleanFiles(files);

            using StringContent jsonContent = new(
                JsonSerializer.Serialize(new
                    {
                        Files = result.Result
                    }
                )
            );

            var httpClient = new HttpClient();
            var response = httpClient.PostAsync("/api/database/insert", jsonContent);

            if (!response.Result.IsSuccessStatusCode)
            {
                return StatusCode(500, response.Result.StatusCode);
            }

            return Ok(result);
        }
        catch (Exception e)
        {
            return StatusCode(500, $"Internal Server Error: {e.Message}");
        }
    }
}