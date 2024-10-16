using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Remote.Webhooks.Handlers.EmploymentHandlers;

public class EmploymentDetailsUpdatedHandler(InvocationContext invocationContext) : BaseWebhookHandler(invocationContext, "employment.details.updated")
{ }