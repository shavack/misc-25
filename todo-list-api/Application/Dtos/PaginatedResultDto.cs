using System.Collections.Generic;

public class PaginatedResultDto<T>
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public IEnumerable<T> Items { get; set; }

    public PaginatedResultDto() { }

    public PaginatedResultDto(IEnumerable<T> items, int page, int pageSize, int totalCount, int totalPages)
    {
        Page = page;
        PageSize = pageSize;
        TotalCount = totalCount;
        TotalPages = totalPages;
        Items = items;
    }
}