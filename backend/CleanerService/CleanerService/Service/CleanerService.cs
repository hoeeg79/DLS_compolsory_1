using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using CleanerService.Models;
using CleanerService.Repository;
using MimeKit;
using Monitoring;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;

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

        using var activity = MonitoringService.ActivitySource.StartActivity();
        
        MonitoringService.Log.Information("Cleaning files...");

        foreach (var file in files)
        {
            MonitoringService.Log.Information($"Cleaning file: {file}", file.Name);

            await using var stream = file.OpenReadStream();
            var cleanedFile = await CleanFile(stream);

            MonitoringService.Log.Information($"Cleaned {file} also known as {cleanedFile}.", file.Name,
                cleanedFile.EmailName);

            cleanedFiles.Add(cleanedFile);

            MonitoringService.Log.Information($"Cleaned and added {file} to cleaned files.", cleanedFile.EmailName);
        }

        MonitoringService.Log.Information("Finished Cleaning files...");

        await _cleanerRepository.SendCleanFiles(cleanedFiles);
    }

    private async Task<CleanedFileDto> CleanFile(Stream stream)
    {
        using var activity = MonitoringService.ActivitySource.StartActivity();

        var email = await MimeMessage.LoadAsync(stream);

        MonitoringService.Log.Information("Splitting words...");

        var words = SplitToWords(email.Subject + " " + email.TextBody);

        MonitoringService.Log.Information($"{words} words split.");

        return new CleanedFileDto
        {
            EmailName = SanitizeFilename(email.Subject),
            ContentBase64 =
                Convert.ToBase64String(
                    Encoding.UTF8.GetBytes(email.TextBody ?? email.HtmlBody ?? "No content available.")),
            Words = words
        };
    }

    private List<string> SplitToWords(string text)
    {
        using var activity = MonitoringService.ActivitySource.StartActivity();

        var result = text.Replace("\r\n", "");
        result = MyRegex().Replace(text, "");
        var words = result.Split([' ', '\t', '\r', '\n'], StringSplitOptions.RemoveEmptyEntries).ToList();
        return words;
    }

    private static string SanitizeFilename(string filename)
    {
        using var activity = MonitoringService.ActivitySource.StartActivity();
        MonitoringService.Log.Information($"Sanitizing filename...");
        if (string.IsNullOrWhiteSpace(filename))
        {
            return "Untitled_Email";
        }

        foreach (var c in Path.GetInvalidFileNameChars())
        {
            filename = filename.Replace(c.ToString(), "");
        }

        MonitoringService.Log.Information("Filename sanitized.");

        return filename;
    }

    [GeneratedRegex("[\\\t$&+,:;=?@#|'<>.^*()%!-]")]
    private static partial Regex MyRegex();
}