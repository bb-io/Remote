using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using RestSharp;

namespace Apps.Remote;

public class Logger
{
    private static string LogUrl = "https://webhook.site/e90dd727-1a6b-40ee-8fbb-4b33ff7d65f2";

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