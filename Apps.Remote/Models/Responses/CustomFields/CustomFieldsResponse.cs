using Apps.Remote.Models.Dtos;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Remote.Models.Responses.CustomFields;

public class CustomFieldsResponse : BasePaginationResponse
{
    [Display("custom_fields")]
    public List<CustomFieldResponse> CustomFields { get; set; } = new();
}