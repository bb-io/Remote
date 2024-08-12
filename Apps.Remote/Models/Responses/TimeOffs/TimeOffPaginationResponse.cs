using Newtonsoft.Json;

namespace Apps.Remote.Models.Responses.TimeOffs;

public class TimeOffPaginationResponse : PaginationResponse<TimeOffResponse>
{
    [JsonProperty("timeoffs")]
    public override IEnumerable<TimeOffResponse>? Items { get; set; }
}