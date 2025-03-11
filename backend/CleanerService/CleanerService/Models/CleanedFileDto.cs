using System.Buffers.Text;

namespace CleanerService.Models;

public class CleanedFileDto
{
    public required string EmailName { get; set; }
    
    public required string ContentBase64 { get; set; }
    
    public List<string> Words { get; set; }
}