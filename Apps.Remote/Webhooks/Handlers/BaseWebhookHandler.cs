using Apps.Remote.Api;
using Apps.Remote.Models.Dtos;
using Apps.Remote.Webhooks.Models;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using RestSharp;

namespace Apps.Remote.Webhooks.Handlers;

public class BaseWebhookHandler(string subscribeEvent) : IWebhookEventHandler
{
    public async Task SubscribeAsync(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProvider, Dictionary<string, string> values)
    {
        var endpoint = "/v1/webhook-callbacks";
        var body = new
        {
            url = values["payloadUrl"],
            subscribed_events = new[] { subscribeEvent }
        };

        var credentials = authenticationCredentialsProvider.ToList();
        var request = new ApiRequest(endpoint, Method.Post, credentials)
            .WithJsonBody(body);
        var apiClient = new ApiClient(credentials);
        
        var response = await apiClient.ExecuteWithErrorHandling<BaseDto<WebhookCallbackDto>>(request);
    }

    public Task UnsubscribeAsync(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProvider, Dictionary<string, string> values)
    {
        throw new NotImplementedException();
    }
}