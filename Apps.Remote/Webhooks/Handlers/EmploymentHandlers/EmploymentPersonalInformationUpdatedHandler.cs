using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Remote.Webhooks.Handlers.EmploymentHandlers;

public class EmploymentPersonalInformationUpdatedHandler(InvocationContext invocationContext) : BaseWebhookHandler(invocationContext, "employment.personal_information.updated")
{ }