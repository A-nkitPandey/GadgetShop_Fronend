using System.ComponentModel.DataAnnotations;

namespace GadgetShop.Models;

public sealed class PagedResult<T>
{
    public List<T> Items { get; set; } = new();
    public int PageNo { get; set; }
    public int PageSize { get; set; }
    public int TotalRecords { get; set; }
    public int TotalPages { get; set; }
}

public class PaginationRequest
{
    [Range(1, int.MaxValue)] public int PageNo { get; set; } = 1;
    [Range(10, int.MaxValue)] public int PageSize { get; set; } = 20;
    public string? SortByColumn { get; set; }
    public bool SortDesOrder { get; set; }
    public string? SearchText { get; set; }
    public string? SearchByColumn { get; set; }
    public List<SearchDomain> Searches { get; set; } = new();
}

public sealed class SearchDomain
{
    public string? PropertyName { get; set; }
    public object? Value { get; set; }
}

public sealed class LookupItem
{
    public long Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}
