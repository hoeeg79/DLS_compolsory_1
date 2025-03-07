using SearchService.Entities;

namespace SearchService.DTO;

public class SearchResultDto
{
    public required List<Files> Files { get; set; }
    public required string SearchQuery { get; set; }
}