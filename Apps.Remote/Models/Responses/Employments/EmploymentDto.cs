using Newtonsoft.Json;

namespace Apps.Remote.Models.Responses.Employments;

public class EmploymentDto
{
    [JsonProperty("employment")]
    public EmploymentResponse Employment { get; set; } = new();
}