using Microsoft.AspNetCore.Mvc;
using SearchService.Repository;

namespace SearchService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SearchController : ControllerBase
{
    private readonly SearchRepository _searchRepository;
    
    public SearchController(SearchRepository searchRepository)
    {
        _searchRepository = searchRepository;
    }
    
    
}