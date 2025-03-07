namespace SearchService.Entities;

public class SearchResult
{
    public required List<Files> Files { get; set; }
    public required string SearchQuery { get; set; }
}