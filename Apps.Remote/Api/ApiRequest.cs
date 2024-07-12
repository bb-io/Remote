using Apps.Remote.Constants;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Utils.Extensions.Sdk;
using Blackbird.Applications.Sdk.Utils.RestSharp;
using RestSharp;

namespace Apps.Remote.Api;

public class ApiRequest(string resource, Method method, IEnumerable<AuthenticationCredentialsProvider> creds)
    : BlackBirdRestRequest(resource, method, creds)
{

    protected override void AddAuth(IEnumerable<AuthenticationCredentialsProvider> creds)
    {
        this.AddHeader("Authorization", $"Bearer {creds.Get(CredsNames.ApiToken).Value}");
    }
}