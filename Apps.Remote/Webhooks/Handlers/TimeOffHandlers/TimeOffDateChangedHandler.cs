using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Remote.Webhooks.Handlers.TimeOffHandlers;

public class TimeOffDateChangedHandler(InvocationContext invocationContext) : BaseWebhookHandler(invocationContext, "timeoff.date_changed")
{ }