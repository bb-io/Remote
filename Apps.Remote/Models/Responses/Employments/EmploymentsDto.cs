using Apps.Remote.Models.Dtos;
using Newtonsoft.Json;

namespace Apps.Remote.Models.Responses.Employments;

public class EmploymentsResponse : BasePaginationResponse
{
    [JsonProperty("employments")]
    public List<EmploymentResponse>? Employments { get; set; } = new();
}