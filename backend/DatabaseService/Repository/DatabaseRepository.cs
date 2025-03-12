using DatabaseService.DTO;
using DatabaseService.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatabaseService.Repository;

public class DatabaseRepository(DatabaseContext context) : IDatabaseRepository
{
    public async Task<SearchResultDto?> GetSearch(string query)
    {
        // Find word
        Words? word = await context.Words.Where(word => word.Word.Contains(query.ToLower())).FirstOrDefaultAsync();
        if (word == null)
        {
            return null;
        }
        
        // Find occurrences
        List<Occurrences> occurrences = await context.Occurrences
            .Where(occurrence => occurrence.WordId == word.WordId)
            .Include(occurrence => occurrence.Email)
            .ToListAsync();
        
        // Generate DTOs
        List<FilesDto> files = occurrences
            .Select(occurrence => new FilesDto
            {
                FileName = occurrence.Email.FileName,
                FileContentBytes = occurrence.Email.FileContent,
                CountOfOccurrences = occurrence.Count
            })
            .ToList();
        SearchResultDto searchResultDto = new SearchResultDto
        {
            Files = files,
            SearchQuery = query
        };

        return searchResultDto;
    }

    public async Task<Emails> InsertFile(Emails email)
    {
        context.Emails.Add(email);
        await context.SaveChangesAsync();
        return email;
    }

    public async Task<List<Words>> InsertWords(List<Words> words)
    {
        context.Words.AddRange(words);
        await context.SaveChangesAsync();
        return words;
    }

    public async Task<Dictionary<string, int>> GetExistingWords(List<string> wordList)
    {
        // Ensure proper query execution by checking against lowercase words
        return await context.Words
            .Where(existingWord => wordList.Contains(existingWord.Word))
            .ToDictionaryAsync(existingWord => existingWord.Word, existingWord => existingWord.WordId);
    }

    public async Task InsertOccurrences(List<Occurrences> occurrences)
    {
        context.Occurrences.AddRange(occurrences);
        await context.SaveChangesAsync();
    }
}