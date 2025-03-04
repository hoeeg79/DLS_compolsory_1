using DatabaseService.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DatabaseController : ControllerBase
{
    private readonly DatabaseRepository _repository;
    
    public DatabaseController(DatabaseRepository repository)
    {
        _repository = repository;
    }
    
}