using CleanerService.Models;
using MimeKit;

namespace CleanerService.Service;

public class CleanerService : ICleanerService
{
    public async Task<List<CleanedFile>> CleanFiles(IFormFile[] files)
    {
        var cleanedFiles = new List<CleanedFile>();
        
        foreach (var file in files)
        {
            await using var stream = file.OpenReadStream();
            var cleanedFile = await CleanFile(stream);
            
            cleanedFiles.Add(cleanedFile);
        }

        return cleanedFiles;
    }

    private async Task<CleanedFile> CleanFile(Stream stream)
    {
        var email = await MimeMessage.LoadAsync(stream);

        return new CleanedFile
        {
            FileName = SanitizeFilename(email.Subject),
            Body = email.TextBody ?? email.HtmlBody ?? "No content available."
        };
    }

    private static string SanitizeFilename(string filename)
    {
        if (string.IsNullOrWhiteSpace(filename))
        {
            return "Untitled_Email";
        }

        foreach (var c in Path.GetInvalidFileNameChars())
        {
            filename = filename.Replace(c.ToString(), "");
        }
        
        return filename;
    }
}