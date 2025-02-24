using Apps.Remote.Models.Dtos;
using Newtonsoft.Json;

namespace Apps.Remote.Models.Responses.CustomFields;

public class CustomFieldsResponse : BasePaginationResponse
{
    [JsonProperty("custom_fields")]
    public List<CustomFieldResponse> CustomFields { get; set; } = new();
}