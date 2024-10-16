using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Remote.Webhooks.Handlers;

public class ContractAmendmentDoneHandler(InvocationContext invocationContext) : BaseWebhookHandler(invocationContext, "contract_amendment.done")
{ }