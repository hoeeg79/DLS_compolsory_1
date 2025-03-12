using System.Text;
using System.Text.RegularExpressions;
using CleanerService.Models;
using CleanerService.Repository;
using MimeKit;

namespace CleanerService.Service;

public partial class CleanerService : ICleanerService
{
    private readonly ICleanerRepository _cleanerRepository;

    public CleanerService(ICleanerRepository cleanerRepository)
    {
        _cleanerRepository = cleanerRepository;
    }

    public async Task CleanFiles(IFormFile[] files)
    {
        var cleanedFiles = new List<CleanedFileDto>();
        
        foreach (var file in files)
        {
            await using var stream = file.OpenReadStream();
            var cleanedFile = await CleanFile(stream);
            
            cleanedFiles.Add(cleanedFile);
        }
        
        await _cleanerRepository.SendCleanFiles(cleanedFiles);
    }
    
    private async Task<CleanedFileDto> CleanFile(Stream stream)
    {
        var email = await MimeMessage.LoadAsync(stream);
        var words = SplitToWords(email.Subject + " " + email.TextBody);

        return new CleanedFileDto
        {
            EmailName = SanitizeFilename(email.Subject),
            ContentBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(email.TextBody ?? email.HtmlBody ?? "No content available.")),
            Words = words
        };
    }
    
    private List<string> SplitToWords(string text)
    {
        var result = text.Replace("\r\n", "");
        result = MyRegex().Replace(text, "");
        var words = result.Split([' ', '\t', '\r', '\n'], StringSplitOptions.RemoveEmptyEntries).ToList();
        return words;
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

    [GeneratedRegex("[\\\t$&+,:;=?@#|'<>.^*()%!-]")]
    private static partial Regex MyRegex();
}