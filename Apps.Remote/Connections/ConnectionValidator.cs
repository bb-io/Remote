using Apps.Remote.Api;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Connections;
using RestSharp;

namespace Apps.Remote.Connections;

public class ConnectionValidator: IConnectionValidator
{
    public async ValueTask<ConnectionValidationResponse> ValidateConnection(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        CancellationToken cancellationToken)
    {
        var credentialsProviders = authenticationCredentialsProviders.ToList();
        var apiClient = new ApiClient(credentialsProviders);
        
        try
        {
            await apiClient.ExecuteAsync(new ApiRequest("/v1/employments", Method.Get, credentialsProviders),
                cancellationToken);
            
            return new ConnectionValidationResponse
            {
                IsValid = true
            };
        }
        catch (Exception e)
        {
            return new ConnectionValidationResponse
            {
                IsValid = false,
                Message = e.Message
            };
        }
    }
}