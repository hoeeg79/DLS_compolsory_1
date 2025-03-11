using Microsoft.AspNetCore.Mvc;
using SearchService.DTO;

namespace SearchService.Repository;

public interface ISearchRepository
{
    /**
     * Method to call the DB API and get the search results
     */
    Task<SearchResultDto?> GetSearch(string searchQuery);
}