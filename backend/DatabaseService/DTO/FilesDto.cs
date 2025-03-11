using Newtonsoft.Json;

namespace DatabaseService.DTO;

public class FilesDto
{
    public required string FileName { get; set; }
    
    [JsonIgnore]
    public required byte[] FileContentBytes { get; set; }
    
    public int CountOfOccurrences { get; set; }
    
    public string FileContent => Convert.ToBase64String(FileContentBytes);
}