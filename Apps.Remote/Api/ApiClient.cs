using Apps.Remote.Utils;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Utils.RestSharp;
using Newtonsoft.Json;
using RestSharp;

namespace Apps.Remote.Api;

public class ApiClient(IEnumerable<AuthenticationCredentialsProvider> creds)
    : BlackBirdRestClient(new RestClientOptions { BaseUrl = creds.GetUrl(), ThrowOnAnyError = true })
{
    protected override JsonSerializerSettings JsonSettings => 
        new() { MissingMemberHandling = MissingMemberHandling.Ignore };

    protected override Exception ConfigureErrorException(RestResponse response)
    {
        return new($"Status code: {response.StatusCode}, Content: {response.Content}");
    }
}

/*
public async Task<T> ExecuteWithJson<T>(string endpoint, Method method, object? body, IEnumerable<AuthenticationCredentialsProvider> credentials)
    {
        var response = await Execute(endpoint, method, body, credentials.ToList());
        var result = JsonConvert.DeserializeObject<T>(response.Content!)!;
        return result;
    }
    
    public async Task Execute(string endpoint, Method method, object? body, IEnumerable<AuthenticationCredentialsProvider> credentials)
    {
        await Execute(endpoint, method, body, credentials.ToList());
    }

    public async Task<RestResponse> Execute(string endpoint, Method method, object? bodyObj, List<AuthenticationCredentialsProvider> creds)
    {
        var baseUri = creds.GetUrl();
        RestRequest request = new ApiRequest(new()
        {
            Url = baseUri + endpoint,
            Method = method
        }, creds);

        if (bodyObj != null)
        {
            request.WithJsonBody(bodyObj, new()
            {
                ContractResolver = new DefaultContractResolver()
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                },
                NullValueHandling = NullValueHandling.Ignore
            });        
        }

        return await ExecuteRequest(request);
    }
    
    private async Task<RestResponse> ExecuteRequest(RestRequest request)
    {
        var response = await ExecuteAsync(request);
        if (!response.IsSuccessStatusCode)
            throw GetError(response);

        return response;
    }
    
    private static Exception GetError(RestResponse response)
    {
        return new($"Status code: {response.StatusCode}, Content: {response.Content}");
    }
 */