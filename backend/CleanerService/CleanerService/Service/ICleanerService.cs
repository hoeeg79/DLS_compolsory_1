using CleanerService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CleanerService.Service;

public interface ICleanerService
{
    Task CleanFiles(IFormFile[] files);
}