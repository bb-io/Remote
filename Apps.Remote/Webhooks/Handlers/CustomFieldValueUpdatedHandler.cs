using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Remote.Webhooks.Handlers;

public class CustomFieldValueUpdatedHandler(InvocationContext invocationContext) : BaseWebhookHandler(invocationContext, "custom_field.value_updated")
{ }