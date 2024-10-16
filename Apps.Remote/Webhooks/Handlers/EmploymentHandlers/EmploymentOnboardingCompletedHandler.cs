using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Remote.Webhooks.Handlers.EmploymentHandlers;

public class EmploymentOnboardingCompletedHandler(InvocationContext invocationContext) : BaseWebhookHandler(invocationContext, "employment.onboarding.completed")
{ }