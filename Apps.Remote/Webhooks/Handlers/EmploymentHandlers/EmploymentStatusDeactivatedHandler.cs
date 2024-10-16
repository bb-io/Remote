using Apps.Remote.Actions;
using Apps.Remote.Models.Identifiers;
using Apps.Remote.Models.Responses.Employments;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.Remote.Webhooks.Handlers.EmploymentHandlers;

public class EmploymentStatusDeactivatedHandler(
    InvocationContext invocationContext,
    [WebhookParameter] EmploymentOptionalIdentifier optionalIdentifier)
    : BaseWebhookHandler(invocationContext, "employment.user_status.deactivated"),
        IAfterSubscriptionWebhookEventHandler<EmploymentResponse>
{
    public async Task<AfterSubscriptionEventResponse<EmploymentResponse>> OnWebhookSubscribedAsync()
    {
        if (optionalIdentifier.EmploymentId == null)
        {
            return null!;
        }

        var employmentActions = new EmploymentActions(InvocationContext);
        var employment =
            await employmentActions.GetEmployment(new() { EmploymentId = optionalIdentifier.EmploymentId });

        if (employment.Status == "inactive")
        {
            return new AfterSubscriptionEventResponse<EmploymentResponse>
            {
                Result = employment
            };
        }

        return null!;
    }
}