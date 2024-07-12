using Apps.Remote.Constants;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Utils.Extensions.Sdk;

namespace Apps.Remote.Utils;

public static class CredentialsExtensions
{
    public static Uri GetUrl(this IEnumerable<AuthenticationCredentialsProvider> creds)
    {
        var url = creds.Get(CredsNames.BaseUrl).Value;
        if(url.EndsWith("/"))
        {
            url = url.Substring(0, url.Length - 1);
        }
        
        return new Uri(url);
    }
}