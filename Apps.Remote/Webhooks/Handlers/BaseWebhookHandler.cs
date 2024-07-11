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
        values.Add(values["payloadUrl"], response.Data!.WebhookCallback.Id);
    }

    public async Task UnsubscribeAsync(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProvider, Dictionary<string, string> values)
    {
        var logRequest = new RestRequest(string.Empty, Method.Post)
            .WithJsonBody(values);
        var logClient = new RestClient("https://webhook.site/909992e4-b83f-4315-8824-8c239797024b");
        await logClient.ExecuteAsync(logRequest);
        
        if(values.TryGetValue(values["payloadUrl"], out var id))
        {
            var endpoint = $"/v1/webhook-callbacks/{id}";
            var credentials = authenticationCredentialsProvider.ToList();
            var request = new ApiRequest(endpoint, Method.Delete, credentials);
            var apiClient = new ApiClient(credentials);
            
            await apiClient.ExecuteWithErrorHandling(request);
        }
        
        await Task.CompletedTask;
    }
}