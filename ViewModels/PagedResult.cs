namespace FlyJusticeLite.ViewModels;

public sealed class PagedResult<T>
{
    public PagedResult(IReadOnlyList<T> items, int totalItems, int pageNumber, int pageSize)
    {
        Items = items;
        TotalItems = totalItems;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }

    public IReadOnlyList<T> Items { get; }

    public int TotalItems { get; }

    public int PageNumber { get; }

    public int PageSize { get; }

    public int TotalPages => Math.Max(1, (int)Math.Ceiling(TotalItems / (double)PageSize));

    public bool HasPreviousPage => PageNumber > 1;

    public bool HasNextPage => PageNumber < TotalPages;

    public static PagedResult<T> Empty(int pageNumber, int pageSize) => new([], 0, pageNumber, pageSize);
}
