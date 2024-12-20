using Apps.Remote.Models.Identifiers;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.Remote.Webhooks.Handlers.TimeOffHandlers;

public class TimeOffCanceledHandler(
    InvocationContext invocationContext,
    [WebhookParameter] TimeOffOptionalIdentifier optionalIdentifier) 
    : BaseTimeOffHandler(invocationContext, optionalIdentifier, status: "canceled", eventName: "timeoff.canceled");