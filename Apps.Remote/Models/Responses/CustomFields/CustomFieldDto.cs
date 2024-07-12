using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.Remote.Models.Responses.CustomFields;

public class CustomFieldDto
{
    [JsonProperty("custom_field_value")]
    public CustomFieldValueResponse CustomFieldValue { get; set; } = new();
}

public class CustomFieldValueResponse
{
    [Display("Custom field ID"), JsonProperty("custom_field_id")]
    public string CustomFieldId { get; set; } = string.Empty;
    
    [Display("Value"), JsonProperty("value")]
    public string? Value { get; set; } = string.Empty;
}