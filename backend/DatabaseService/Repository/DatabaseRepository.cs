using DatabaseService.DTO;
using DatabaseService.Entities;

namespace DatabaseService.Repository;

public class DatabaseRepository : IDatabaseRepository
{
    private readonly DatabaseContext _context;
    
    public DatabaseRepository(DatabaseContext context)
    {
        _context = context;
    }
    
    public Files GetFile(int fileId)
    {
        throw new NotImplementedException();
    }

    public Files GetFile(string fileName)
    {
        throw new NotImplementedException();
    }

    public Words GetWord(int wordId)
    {
        throw new NotImplementedException();
    }

    public Occurrences GetOccurrence(Words word)
    {
        throw new NotImplementedException();
    }

    public async Task<SearchResultDto?> GetSearch(string query)
    {
        throw new NotImplementedException();
    }
}