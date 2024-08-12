namespace Apps.Remote.Models.Responses;

public abstract class PaginationResponse<T>
{
    public abstract IEnumerable<T>? Items { get; set; }
    
    public int TotalPages { get; set; }

    public int TotalCount { get; set; }
}