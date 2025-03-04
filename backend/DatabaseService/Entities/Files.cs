
namespace DatabaseService.Entities;

public class Files
{
    public int FileId { get; set; }
    
    public required string FileName { get; set; }
    
    public required byte[] FileContent { get; set; }
    
}