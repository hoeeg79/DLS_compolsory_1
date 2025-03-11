using Newtonsoft.Json;

namespace DatabaseService.DTO;

public class ScrubbedFilesDto
{
    public required string EmailName { get; set; }
    public required string ContentBase64 { get; set; } // Base64 string
    public required List<string> Words { get; set; }

    [JsonIgnore]
    public byte[] Content => Convert.FromBase64String(ContentBase64);
}