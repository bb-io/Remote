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
    [Webhook("On employment activated", typeof(EmploymentActivatedHandler),
        Description = "This event is triggered whenever an employment user is updated to the active status")]
    public Task<WebhookResponse<EmploymentResponse>> OnEmploymentActivated(WebhookRequest webhookRequest,
        [WebhookParameter] EmploymentOptionalIdentifier employmentOptionalIdentifier)   
    {
        return HandleEmploymentWebhook(webhookRequest, employmentOptionalIdentifier);
    }

    [Webhook("On employment onboarding completed", typeof(EmploymentOnboardingCompletedHandler),
        Description = "This event is triggered whenever an employment user has completed onboarding")]
    public Task<WebhookResponse<EmploymentResponse>> OnEmploymentOnboardingCompleted(WebhookRequest webhookRequest,
        [WebhookParameter] EmploymentOptionalIdentifier employmentOptionalIdentifier)   
    {
        return HandleEmploymentWebhook(webhookRequest, employmentOptionalIdentifier);
    }

    [Webhook("On employment details updated", typeof(EmploymentDetailsUpdatedHandler),
        Description = "This event is triggered whenever an employment user's details are updated")]
    public Task<WebhookResponse<EmploymentResponse>> OnEmploymentDetailsUpdated(WebhookRequest webhookRequest,
        [WebhookParameter] EmploymentOptionalIdentifier employmentOptionalIdentifier)   
    {
        return HandleEmploymentWebhook(webhookRequest, employmentOptionalIdentifier);
    }

    [Webhook("On employment personal information updated", typeof(EmploymentPersonalInformationUpdatedHandler),
        Description = "This event is triggered whenever an employment user's personal information is updated")]
    public Task<WebhookResponse<EmploymentResponse>> OnEmploymentPersonalInformationUpdated(WebhookRequest webhookRequest,
        [WebhookParameter] EmploymentOptionalIdentifier employmentOptionalIdentifier)   
    {
        return HandleEmploymentWebhook(webhookRequest, employmentOptionalIdentifier);
    }

    [Webhook("On employment status deactivated", typeof(EmploymentStatusDeactivatedHandler),
        Description = "This event is triggered whenever an employment user status is updated to inactive")]
    public Task<WebhookResponse<EmploymentResponse>> OnEmploymentStatusDeactivated(WebhookRequest webhookRequest,
        [WebhookParameter] EmploymentOptionalIdentifier employmentOptionalIdentifier)   
    {
        return HandleEmploymentWebhook(webhookRequest, employmentOptionalIdentifier);
    }

    private async Task<WebhookResponse<EmploymentResponse>> HandleEmploymentWebhook(WebhookRequest webhookRequest, EmploymentOptionalIdentifier optionalIdentifier)
    {
        var payload = webhookRequest.Body.ToString()!;
        if (string.IsNullOrEmpty(payload))
        {
            throw new Exception("Payload is empty");
        }

        var employmentPayload = JsonConvert.DeserializeObject<EmploymentPayload>(payload) ??
                                throw new Exception($"Failed to deserialize payload: {payload}");
        
        if(optionalIdentifier.EmploymentId != null && employmentPayload.EmploymentId != optionalIdentifier.EmploymentId)
        {
            return new WebhookResponse<EmploymentResponse>
            {
                Result = null,
                ReceivedWebhookRequestType = WebhookRequestType.Default
            };
        }

        var employmentActions = new EmploymentActions(InvocationContext);
        var employment = await employmentActions.GetEmployment(new EmploymentIdentifier
            { EmploymentId = employmentPayload.EmploymentId });

        return new WebhookResponse<EmploymentResponse>
        {
            Result = employment,
            ReceivedWebhookRequestType = WebhookRequestType.Default
        };
    }
}