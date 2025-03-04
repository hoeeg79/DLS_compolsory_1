using Microsoft.AspNetCore.Mvc;

namespace CleanerService.Service;

public interface ICleanerService
{
    Task<ActionResult> CleanFiles(IFormFile[] files);
}