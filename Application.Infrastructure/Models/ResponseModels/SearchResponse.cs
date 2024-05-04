namespace Application.Infrastructure.Models.ResponseModels;

public class SearchResponse<T>
{
    public IReadOnlyCollection<T> Data { get; set; } = new List<T>();
    public int PageSize { get; set; }
    public int Page { get; set; }
    public long TotalCount { get; set; }
    public long PageCount { get; set; }
}

public class SearchRequest
{
    public int PageSize { get; set; } = 20;
    public int Page { get; set; } = 1;
}