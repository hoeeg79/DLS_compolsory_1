using DatabaseService.Entities;

namespace DatabaseService.Repository;

public interface IDatabaseRepository
{
    Files GetFile(int fileId);
    
    Files GetFile(string fileName);
    
    Words GetWord(int wordId);
    
    Occurrences GetOccurrence(Words word);
}