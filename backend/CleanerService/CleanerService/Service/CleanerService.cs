using System.Text.RegularExpressions;
using CleanerService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CleanerService.Service;

public class CleanerService : ICleanerService
{
    public async Task<ActionResult> CleanFiles(IFormFile[] files)
    {
        var cleanedFiles = new List<CleanedFile>();
        
        foreach (var file in files)
        {
            try
            {
                using var stream = new MemoryStream();
                await file.CopyToAsync(stream);
                var fileContent = System.Text.Encoding.UTF8.GetString(stream.ToArray());

                var cleanedFile = CleanFileContent(fileContent);
                var fileName = file.Name;
                    
                cleanedFiles.Add(new CleanedFile
                {
                    FileName = fileName,
                    Body = cleanedFile
                });
            }
            catch (Exception e)
            {
                cleanedFiles.Add(new CleanedFile
                {
                    FileName = file.FileName,
                    Body = $@"Error processing file: {e.Message}"
                });
            }
            Console.WriteLine("Files cleaned: " + cleanedFiles.Count);
        }

        return new OkObjectResult(cleanedFiles);
    }

    private string CleanFileContent(string content)
    {
        var pattern = @"(?<=\n)[^@]*\n.*?\n--\n";
        var body = Regex.Replace(content, pattern, "", RegexOptions.Singleline);

        return body.Trim();
    }
}