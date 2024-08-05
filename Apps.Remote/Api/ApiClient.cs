using Apps.Remote.Constants;
using Apps.Remote.Models.Dtos;
using Apps.Remote.Models.Responses;
using Apps.Remote.Utils;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using Blackbird.Applications.Sdk.Utils.RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace Apps.Remote.Api;

public class ApiClient(IEnumerable<AuthenticationCredentialsProvider> creds)
    : BlackBirdRestClient(new RestClientOptions { BaseUrl = creds.GetUrl(), ThrowOnAnyError = false })
{
    private const int PageSize = 100;

    protected override JsonSerializerSettings JsonSettings => JsonConfig.JsonSettings;

    protected override Exception ConfigureErrorException(RestResponse response)
    {
        return ConfigureException(response);
    }

    public async Task<List<T>> Paginate<T, TV>(RestRequest request) where TV : PaginationResponse<T>
    {
        var result = new List<T>();
        var currentPage = 1;
        var baseUrl = request.Resource.SetQueryParameter("page_size", PageSize.ToString());
        PaginationResponse<T> response;

        do
        {
            request.Resource = baseUrl
                .SetQueryParameter("page", currentPage.ToString());

            response = (await ExecuteWithErrorHandling<BaseDto<TV>>(request)).Data!;

            if (response?.Items != null)
                result.AddRange(response.Items);

            currentPage++;
        } while (currentPage <= response.TotalPages);

        return result;
    }
    
    public Exception ConfigureException(RestResponse response)
    {
        string errorMessage = $"Status code: {response.StatusCode}, ";

        try
        {
            var responseObject = JsonConvert.DeserializeObject<JObject>(response.Content!)!;

            // Check if there's a 'data' object
            if (responseObject["data"] is JObject dataObject)
            {
                var failures = dataObject["failures"]?.ToObject<JArray>();
                if (failures != null && failures.Count > 0)
                {
                    foreach (var failure in failures)
                    {
                        var number = failure["number"]?.ToString();
                        var errors = failure["errors"]?.ToObject<JObject>();

                        if (errors != null && errors.Count > 0)
                        {
                            errorMessage += $"Failure number: {number}, Errors: ";
                            foreach (var error in errors)
                            {
                                var key = error.Key;
                                var messages = error.Value?.ToObject<JArray>();

                                if (messages != null && messages.Count > 0)
                                {
                                    errorMessage += $"{key}: {string.Join(", ", messages.Select(e => e.ToString()))}; ";
                                }
                            }
                        }
                    }
                }
                else
                {
                    errorMessage += $"Message: No specific failure details found.";
                }
            }
            // Check if there's an 'errors' object directly
            else if (responseObject["errors"] is JObject errorsObject)
            {
                if (errorsObject.Count > 0)
                {
                    errorMessage += "Errors: ";
                    foreach (var error in errorsObject)
                    {
                        var key = error.Key;
                        var messages = error.Value?.ToObject<JArray>();

                        if (messages != null && messages.Count > 0)
                        {
                            errorMessage += $"{key}: {string.Join(", ", messages.Select(e => e.ToString()))}; ";
                        }
                    }
                }
                else
                {
                    errorMessage += $"Message: No specific error details found.";
                }
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

        return new Exception(errorMessage.Trim());
    }
}