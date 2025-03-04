namespace DatabaseService.Entities;

public class Occurrences
{
    public required int WordId { get; set; }
    
    public required int FileId { get; set; }
    
    public required int Count { get; set; }
    
    public Words Word { get; set; }
    
    public Files File { get; set; }
    
}