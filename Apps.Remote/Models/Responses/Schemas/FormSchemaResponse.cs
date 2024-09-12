using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Apps.Remote.Models.Responses.Schemas;

public class FormSchemaResponse
{
    [JsonProperty("required")]
    public List<string> RequiredProperties { get; set; } = new();

    [JsonProperty("properties")]
    public JObject Properties { get; set; } = new();
}