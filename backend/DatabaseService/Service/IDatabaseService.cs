using DatabaseService.DTO;

namespace DatabaseService.Service;

public interface IDatabaseService
{
    Task<SearchResultDto?> GetSearch(string query);
    Task InsertFiles(ListScrubbedFilesDto fileName);
}