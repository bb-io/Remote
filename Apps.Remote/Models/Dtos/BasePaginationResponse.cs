using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.Remote.Models.Dtos;

public class BasePaginationResponse
{
    [JsonProperty("total_count")]
    [Display("Total count")]
    public double TotalCount { get; set; }

    [DefinitionIgnore, JsonProperty("current_page")]
    public int CurrentPage { get; set; }

    [DefinitionIgnore, JsonProperty("total_pages")]
    public int TotalPages { get; set; }
}
