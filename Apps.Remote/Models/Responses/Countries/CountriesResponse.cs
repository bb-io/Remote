using Newtonsoft.Json;

namespace Apps.Remote.Models.Responses.Countries;

public class CountriesResponse
{
    [JsonProperty("data")]
    public List<CountryResponse> Data { get; set; } = new();
}