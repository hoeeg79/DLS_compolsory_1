using CleanerService.Models;

namespace CleanerService.Repository;

public interface ICleanerRepository
{
    Task SendCleanFiles(List<CleanedFileDto> files);
}