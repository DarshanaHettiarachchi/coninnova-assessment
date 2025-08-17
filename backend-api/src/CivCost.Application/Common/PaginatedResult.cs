namespace CivCost.Application.Common;
public record PaginatedResult<T>(
    IEnumerable<T> Items,
    int CurrentPage,
    int PageSize,
    int TotalItems,
    int TotalPages
)
{
    public bool HasNextPage => CurrentPage < TotalPages;
    public bool HasPreviousPage => CurrentPage > 1;
}
