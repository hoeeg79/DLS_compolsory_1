using CleanerService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CleanerService.Service;

public interface ICleanerService
{
    Task<List<CleanedFile>> CleanFiles(IFormFile[] files);
}