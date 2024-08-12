using Apps.Remote.Api;
using Apps.Remote.Constants;
using Apps.Remote.Invocables;
using Apps.Remote.Models.Dtos;
using Apps.Remote.Models.Responses.TimeOffs;
using Apps.Remote.Webhooks.Handlers.TimeOffHandlers;
using Apps.Remote.Webhooks.Payload;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Newtonsoft.Json;
using RestSharp;

namespace Apps.Remote.Webhooks;

[WebhookList]
public class TimeOffWebhookList(InvocationContext invocationContext) : AppInvocable(invocationContext)
{
    [Webhook("On time off canceled", typeof(TimeOffCanceledHandler),
        Description = "Triggers when a time off is canceled")]
    public Task<WebhookResponse<TimeOffResponse>> OnTimeOffCanceled(WebhookRequest webhookRequest) =>
        HandleEmploymentWebhook(webhookRequest);
    
    [Webhook("On time off declined", typeof(TimeOffDeclinedHandler),
        Description = "Triggers when a time off is declined")]
    public Task<WebhookResponse<TimeOffResponse>> OnTimeOffDeclined(WebhookRequest webhookRequest) =>
        HandleEmploymentWebhook(webhookRequest);
    
    [Webhook("On time off requested", typeof(TimeOffRequestedHandler),
        Description = "Triggers when a time off is requested")]
    public Task<WebhookResponse<TimeOffResponse>> OnTimeOffRequested(WebhookRequest webhookRequest) =>
        HandleEmploymentWebhook(webhookRequest);
    
    [Webhook("On time off date changed", typeof(TimeOffDateChangedHandler),
        Description = "Triggers when a time off has its date changed")]
    public Task<WebhookResponse<TimeOffResponse>> OnTimeOffDateChanged(WebhookRequest webhookRequest) =>
        HandleEmploymentWebhook(webhookRequest);

    private async Task<WebhookResponse<TimeOffResponse>> HandleEmploymentWebhook(WebhookRequest webhookRequest)
    {
        var payload = webhookRequest.Body.ToString()!;
        if (string.IsNullOrEmpty(payload))
        {
            throw new Exception("Payload is empty");
        }

        var timeOffPayload = JsonConvert.DeserializeObject<TimeOffPayload>(payload, JsonConfig.JsonSettings) ??
                             throw new Exception($"Failed to deserialize payload: {payload}");

        var apiRequest = new ApiRequest($"/v1/timeoff/{timeOffPayload.TimeoffId}", Method.Get, Creds);
        var response = await Client.ExecuteWithErrorHandling<BaseDto<TimeOffDto>>(apiRequest);

        return new WebhookResponse<TimeOffResponse>
        {
            Result = response.Data?.Timeoff!,
            ReceivedWebhookRequestType = WebhookRequestType.Default
        };
    }
}