using Newtonsoft.Json;

namespace Apps.Remote.Webhooks.Models;

public class WebhookCallbackResponse
{
    [JsonProperty("url")] 
    public string Url { get; set; } = string.Empty;
    
    [JsonProperty("signing_key")]
    public string SigningKey { get; set; } = string.Empty;
    
    [JsonProperty("subscribed_events")]
    public string[] SubscribedEvents { get; set; } = Array.Empty<string>();
}