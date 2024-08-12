using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using RestSharp;

namespace Apps.Remote;

public class Logger
{
    private static string LogUrl = "https://webhook.site/d05d7a10-f7cd-4917-8ba2-a30ee72b6719";

    public static async Task LogAsync<T>(T obj) where T : class
    {
        var restRequest = new RestRequest(string.Empty, Method.Post)
            .WithJsonBody(obj);
        var restClient = new RestClient(LogUrl);
        
        await restClient.ExecuteAsync(restRequest);
    }
    
    public static Task LogAsync(Exception ex)
    {
        return LogAsync(new { Status = "Exception", ex.Message, ex.StackTrace, Type = ex.GetType().Name});
    }
}