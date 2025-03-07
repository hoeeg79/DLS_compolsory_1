namespace DatabaseService.DTO;

public class FilesDto
{
    public required string FileName { get; set; }
    
    public required byte[] FileContent { get; set; }
    
    public int CountOfOccurrences { get; set; }
}