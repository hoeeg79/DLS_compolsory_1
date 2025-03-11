using DatabaseService.DTO;
using DatabaseService.Entities;
using DatabaseService.Repository;

namespace DatabaseService.Service;

public class DatabaseService(IDatabaseRepository repository) : IDatabaseService
{
    public async Task<SearchResultDto?> GetSearch(string query)
    {
        return await repository.GetSearch(query);
    }

    public async Task InsertFiles(ListScrubbedFilesDto fileName)
    {
        foreach (ScrubbedFilesDto scrubbedFile in fileName.Files)
        {
            // Save file
            Emails createdEmail = await SaveEmail(scrubbedFile.EmailName, scrubbedFile.Content);
            
            // 2. Normalize words before processing
            List<string> cleanedWords = scrubbedFile.Words
                .Select(word => word.Trim().ToLower()) // Trim spaces and normalize casing
                .ToList();

            // 3. Save words in the DB
            Dictionary<string, int> wordDictionary = await SaveWords(cleanedWords);

            // 4. Fix word count issue: Group words properly before inserting
            List<Occurrences> occurrencesList = cleanedWords
                .GroupBy(word => word) // Group by normalized word
                .Select(groupedWord => new Occurrences
                {
                    FileId = createdEmail.FileId,
                    WordId = wordDictionary[groupedWord.Key], // Get correct word ID
                    Count = groupedWord.Count() // Proper count of occurrences
                })
                .ToList();

            await repository.InsertOccurrences(occurrencesList);
        }
    }

    private async Task<Dictionary<string, int>> SaveWords(List<string> words)
    {
        // Normalize words and remove duplicates
        var uniqueWordsList = words
            .Select(word => word.ToLower())
            .Distinct()
            .ToList();

        // Fetch existing words from DB
        Dictionary<string, int> existingWordsDictionary = await repository.GetExistingWords(uniqueWordsList);

        // Filter out new words that are not in the database
        var newWordsList = uniqueWordsList
            .Where(word => !existingWordsDictionary.ContainsKey(word))
            .Select(word => new Words { Word = word })
            .ToList();

        if (newWordsList.Count > 0)
        {
            // Insert new words and update dictionary
            var insertedWords = await repository.InsertWords(newWordsList);
            foreach (var newWord in insertedWords)
            {
                existingWordsDictionary[newWord.Word] = newWord.WordId;
            }
        }

        return existingWordsDictionary;
    }

    private async Task<Emails> SaveEmail(string fileName, byte[] content)
    {
        Emails email = new()
        {
            FileName = fileName,
            FileContent = content
        };
        return await repository.InsertFile(email);
    }
}