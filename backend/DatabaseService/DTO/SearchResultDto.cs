namespace DatabaseService.DTO;

public class SearchResultDto
{
    public required List<FilesDto> Files { get; set; }
    
    public required string SearchQuery { get; set; }
}