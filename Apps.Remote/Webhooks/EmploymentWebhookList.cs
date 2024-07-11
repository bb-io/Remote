using Apps.Remote.Actions;
using Apps.Remote.Invocables;
using Apps.Remote.Models.Identifiers;
using Apps.Remote.Models.Responses.Employments;
using Apps.Remote.Webhooks.Handlers.EmploymentHandlers;
using Apps.Remote.Webhooks.Payload;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Newtonsoft.Json;

namespace Apps.Remote.Webhooks;

[WebhookList]
public class EmploymentWebhookList(InvocationContext invocationContext) : AppInvocable(invocationContext)
{
    [Webhook("On employment activated", typeof(EmploymentActivatedHandler), Description = "This event is triggered whenever an employment user is updated to the active status")]
    public Task<WebhookResponse<EmploymentResponse>> OnEmploymentActivated(WebhookRequest webhookRequest)
    {
        return HandleEmploymentWebhook(webhookRequest);
    }

    [Webhook("On employment onboarding completed", typeof(EmploymentOnboardingCompletedHandler), Description = "This event is triggered whenever an employment user has completed onboarding")]
    public Task<WebhookResponse<EmploymentResponse>> OnEmploymentOnboardingCompleted(WebhookRequest webhookRequest)
    {
        return HandleEmploymentWebhook(webhookRequest);
    }

    [Webhook("On employment details updated", typeof(EmploymentDetailsUpdatedHandler), Description = "This event is triggered whenever an employment user's details are updated")]
    public Task<WebhookResponse<EmploymentResponse>> OnEmploymentDetailsUpdated(WebhookRequest webhookRequest)
    {
        return HandleEmploymentWebhook(webhookRequest);
    }

    [Webhook("On employment personal information updated", typeof(EmploymentPersonalInformationUpdatedHandler), Description = "This event is triggered whenever an employment user's personal information is updated")]
    public Task<WebhookResponse<EmploymentResponse>> OnEmploymentPersonalInformationUpdated(WebhookRequest webhookRequest)
    {
        return HandleEmploymentWebhook(webhookRequest);
    }
    
    private async Task<WebhookResponse<EmploymentResponse>> HandleEmploymentWebhook(WebhookRequest webhookRequest)
    {
        var payload = webhookRequest.Body.ToString()!;
        if (string.IsNullOrEmpty(payload))
        {
            throw new Exception("Payload is empty");
        }

        var employmentPayload = JsonConvert.DeserializeObject<EmploymentPayload>(payload) ?? throw new Exception($"Failed to deserialize payload: {payload}");
        
        var employmentActions = new EmploymentActions(InvocationContext);
        var employment = await employmentActions.GetEmployment(new EmploymentIdentifier { EmploymentId = employmentPayload.EmploymentId });
        
        return new WebhookResponse<EmploymentResponse>
        {
            Result = employment,
            ReceivedWebhookRequestType = WebhookRequestType.Default
        };  
    }
}