using Newtonsoft.Json;

namespace Apps.Remote.Models.Dtos;

public class OneOfDto
{
    [JsonProperty("const")]
    public string Constant { get; set; } = string.Empty;
    
    [JsonProperty("title")]
    public string Title { get; set; } = string.Empty;
}