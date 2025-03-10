using System.ComponentModel.DataAnnotations;

namespace Tourism_Api.Pagnations;

public record RequestFilters
{
    [Range(1, int.MaxValue, ErrorMessage = "PageNumber Value must be at least 1.")]
    public int PageNumber { get; init; } = 1;
    [Range(1, int.MaxValue, ErrorMessage = "PageSize Value must be at least 1.")]
    public int PageSize { get; init; } = 100;
    public string? SearchValue { get; init; } = null;
    //public string? SortColumn { get; init; } = "JoinDate";
    //public string? SortDirection { get; init; } = "ASC";
}