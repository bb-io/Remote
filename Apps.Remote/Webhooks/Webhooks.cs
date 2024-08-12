using Apps.Remote.Api;
using Apps.Remote.Constants;
using Apps.Remote.Invocables;
using Apps.Remote.Models.Dtos;
using Apps.Remote.Models.Responses.CustomFields;
using Apps.Remote.Webhooks.Handlers;
using Apps.Remote.Webhooks.Payload;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Newtonsoft.Json;
using RestSharp;

namespace Apps.Remote.Webhooks;

[WebhookList]
public class Webhooks(InvocationContext invocationContext) : AppInvocable(invocationContext)
{
    [Webhook("On custom field value updated", typeof(CustomFieldValueUpdatedHandler),
        Description = "This event is triggered whenever a contract amendment employment request is fully executed")]
    public async Task<WebhookResponse<CustomFieldValueResponse>> OnCustomFieldValueUpdated(
        WebhookRequest webhookRequest)
    {
        var payload = webhookRequest.Body.ToString()!;
        if (string.IsNullOrEmpty(payload))
        {
            throw new Exception("Payload is empty");
        }

        var customFieldValuePayload = JsonConvert.DeserializeObject<CustomFieldValuePayload>(payload, JsonConfig.JsonSettings) ??
                                      throw new Exception($"Failed to deserialize payload: {payload}");

        var endpoint =
            $"/v1/custom-fields/{customFieldValuePayload.CustomFieldId}/values/{customFieldValuePayload.EmploymentId}";
        var apiRequest = new ApiRequest(endpoint, Method.Get, Creds);
        var response = await Client.ExecuteWithErrorHandling<BaseDto<CustomFieldDto>>(apiRequest);
        if (response.Data != null)
        {
            response.Data.CustomFieldValue.Value ??= string.Empty;
        }

        return new WebhookResponse<CustomFieldValueResponse>
        {
            Result = response.Data?.CustomFieldValue ?? new CustomFieldValueResponse()
                { CustomFieldId = customFieldValuePayload.CustomFieldId, Value = string.Empty },
            ReceivedWebhookRequestType = WebhookRequestType.Default
        };
    }   
    
    // [Webhook("On contract amendment done", typeof(ContractAmendmentDoneHandler),
    //     Description = "This event is triggered whenever a contract amendment employment request is fully executed")]
    // public async Task<WebhookResponse<CustomFieldValueResponse>> OnContractAmendmentDone(WebhookRequest webhookRequest)
    // {
    //     return new();
    // }
}