using DatabaseService.DTO;
using DatabaseService.Entities;

namespace DatabaseService.Repository;

public interface IDatabaseRepository
{
    Task<SearchResultDto?> GetSearch(string query);
    Task<Emails> InsertFile(Emails email);
    Task<List<Words>> InsertWords(List<Words> words);
    Task<Dictionary<string, int>> GetExistingWords(List<string> uniqueWords);
    Task InsertOccurrences(List<Occurrences> occurrences);
}