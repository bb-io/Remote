using RestSharp;

namespace Apps.Remote;

public class Logger
{
    private const string LogUrl = @"https://webhook.site/c3b71282-e328-4abb-9845-e0530412bc55";
    
    public static async Task LogAsync<T>(T obj) where T : class
    {
        var restRequest = new RestRequest(string.Empty, Method.Post)
            .AddJsonBody(obj);
        var restClient = new RestClient(LogUrl);
        await restClient.ExecuteAsync(restRequest);
    }
    
    public static void Log<T>(T obj) where T : class
    {
        var restRequest = new RestRequest(string.Empty, Method.Post)
            .AddJsonBody(obj);
        var restClient = new RestClient(LogUrl);
        restClient.Execute(restRequest);
    }
 }