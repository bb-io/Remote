using Apps.Remote.Utils;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Utils.RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace Apps.Remote.Api;

public class ApiClient(IEnumerable<AuthenticationCredentialsProvider> creds)
    : BlackBirdRestClient(new RestClientOptions { BaseUrl = creds.GetUrl(), ThrowOnAnyError = true })
{
    protected override JsonSerializerSettings JsonSettings => 
        new() { MissingMemberHandling = MissingMemberHandling.Ignore };

    protected override Exception ConfigureErrorException(RestResponse response)
    {
        return ConfigureException(response);
    }
    
    public Exception ConfigureException(RestResponse response)
    {
        string errorMessage = $"Status code: {response.StatusCode}, ";

        try
        {
            var responseObject = JsonConvert.DeserializeObject<JObject>(response.Content!)!;

            if (responseObject["message"] is JObject messageObject)
            {
                var code = messageObject["code"]?.ToString();
                var message = messageObject["message"]?.ToString();
                var param = messageObject["param"]?.ToString();
            
                errorMessage += $"Error code: {code}, Message: {message}, Parameter: {param}";
            }
            else
            {
                var message = responseObject["message"]?.ToString();
                errorMessage += $"Message: {message}";
            }
        }
        catch (JsonException)
        {
            errorMessage += $"Content: {response.Content}";
        }

        return new Exception(errorMessage);
    }
}
