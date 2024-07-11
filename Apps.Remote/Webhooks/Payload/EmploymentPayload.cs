using Newtonsoft.Json;

namespace Apps.Remote.Webhooks.Payload;

public class EmploymentPayload
{
    [JsonProperty("employment_id")] 
    public string EmploymentId { get; set; } = string.Empty;

    [JsonProperty("event_type")] 
    public string EventType { get; set; } = string.Empty;
}