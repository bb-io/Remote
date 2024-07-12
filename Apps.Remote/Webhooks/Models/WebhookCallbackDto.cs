using Newtonsoft.Json;

namespace Apps.Remote.Webhooks.Models;

public class WebhookCallbackDto
{
    [JsonProperty("webhook_callback")]
    public WebhookCallbackResponse WebhookCallback { get; set; } = new();
}