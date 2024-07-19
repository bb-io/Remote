using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.Remote.Models.Responses.CustomFields;

public class CustomFieldResponse
{
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;
    
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;
    
    [JsonProperty("type")]
    public string Type { get; set; } = string.Empty;
    
    [JsonProperty("required")]
    public bool Required { get; set; }
    
    [JsonProperty("visibility_scope"), Display("Visibility score")]
    public string VisibilityScope { get; set; } = string.Empty;
    
    [JsonProperty("data_entry_access"), Display("Data entry access")]
    public string DataEntryAccess { get; set; } = string.Empty;
}