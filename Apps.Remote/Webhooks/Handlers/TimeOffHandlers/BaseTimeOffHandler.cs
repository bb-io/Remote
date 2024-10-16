using Apps.Remote.Actions;
using Apps.Remote.Models.Identifiers;
using Apps.Remote.Models.Responses.TimeOffs;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.Remote.Webhooks.Handlers.TimeOffHandlers;

public class BaseTimeOffHandler(InvocationContext invocationContext, TimeOffOptionalIdentifier optionalIdentifier, string status, string eventName)
    : BaseWebhookHandler(invocationContext, eventName), IAfterSubscriptionWebhookEventHandler<TimeOffResponse>
{
    public async Task<AfterSubscriptionEventResponse<TimeOffResponse>> OnWebhookSubscribedAsync()
    {
        if (optionalIdentifier.TimeOffId == null)
        {
            return null!;
        }

        var timeOffActions = new TimeOffActions(InvocationContext);
        var timeOff =
            await timeOffActions.GetTimeOff(new() { TimeOffId = optionalIdentifier.TimeOffId });

        if (timeOff.Status == status)
        {
            return new AfterSubscriptionEventResponse<TimeOffResponse>
            {
                Result = timeOff
            };
        }

        return null!;
    }
}