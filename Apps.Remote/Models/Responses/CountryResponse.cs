using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.Remote.Models.Responses;

public class CountryResponse
{
    [JsonProperty("code")]
    public string Code { get; set; } = string.Empty;

    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    [Display("Alpha 2 code"), JsonProperty("alpha_2_code")]
    public string Alpha2Code { get; set; } = string.Empty;

    [DefinitionIgnore, JsonProperty("supported_json_schemas")]
    public List<string> SupportedJsonSchemas { get; set; } = new();
}