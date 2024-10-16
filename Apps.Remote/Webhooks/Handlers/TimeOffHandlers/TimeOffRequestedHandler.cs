using Apps.Remote.Models.Identifiers;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.Remote.Webhooks.Handlers.TimeOffHandlers;

public class TimeOffRequestedHandler(InvocationContext invocationContext,
    [WebhookParameter] TimeOffOptionalIdentifier optionalIdentifier) 
    : BaseTimeOffHandler(invocationContext, optionalIdentifier, status: "requested", eventName: "timeoff.requested");