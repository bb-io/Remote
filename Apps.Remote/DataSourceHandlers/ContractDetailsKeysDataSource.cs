using System.Text.RegularExpressions;
using Apps.Remote.Api;
using Apps.Remote.Invocables;
using Apps.Remote.Models.Dtos;
using Apps.Remote.Models.Identifiers;
using Apps.Remote.Models.Responses.Schemas;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace Apps.Remote.DataSourceHandlers;

public class ContractDetailsKeysDataSource(
    InvocationContext invocationContext,
    [ActionParameter] CountryIdentifier identifier)
    : AppInvocable(invocationContext), IAsyncDataSourceHandler
{
    public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(identifier.CountryCode))
        {
            throw new PluginMisconfigurationException("You should provide a country code first");
        }

        var request = new ApiRequest($"/v1/countries/{identifier.CountryCode}/contract_details", Method.Get, Creds);
        var response = await Client.ExecuteWithErrorHandling<BaseDto<FormSchemaResponse>>(request);

        var result = new Dictionary<string, string>();
        var properties = response.Data?.Properties;

        if (properties != null)
        {
            foreach (var property in properties)
            {
                var propertyName = property.Key;
                var propertyValue = property.Value as JObject;

                var propertyTitle = propertyValue?["title"]?.ToString() ?? propertyName;

                var propertyNameWithType = CleanPropertyName($"[{propertyValue["type"]}]{propertyName}");
                
                if (propertyValue?["properties"] != null)
                {
                    var nestedProperties = propertyValue["properties"] as JObject;
                    ProcessNestedProperties(propertyName, nestedProperties, result);
                }
                else
                {
                    if (propertyValue?["type"]?.ToString().Contains("null") == true)
                    {
                        result.Add(propertyNameWithType, $"(Nullable) {propertyTitle}");
                    }
                    else
                    {
                        result.Add(propertyNameWithType, propertyTitle);
                    }
                }
            }
        }

        return result.Where(x =>
                context.SearchString == null ||
                x.Value.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .ToDictionary(x => x.Key, x => x.Value);
    }

    private void ProcessNestedProperties(string parentPropertyName, JObject nestedProperties,
        Dictionary<string, string> result)
    {
        foreach (var nestedProperty in nestedProperties)
        {
            var nestedPropertyName = nestedProperty.Key;
            var nestedPropertyValue = nestedProperty.Value as JObject;

            var nestedPropertyTitle = nestedPropertyValue?["title"]?.ToString() ?? nestedPropertyName;
            var fullPropertyName = $"{parentPropertyName}.{nestedPropertyName}";

            if (nestedPropertyValue?["properties"] != null)
            {
                ProcessNestedProperties(fullPropertyName, nestedPropertyValue["properties"] as JObject, result);
            }
            else
            {
                var cleanedPropertyName = CleanPropertyName($"[{nestedPropertyValue?["type"]}]{fullPropertyName}");

                if (nestedPropertyValue?["type"]?.ToString().Contains("null") == true)
                {
                    result.Add(cleanedPropertyName, $"(Nullable) {nestedPropertyTitle}");
                }
                else
                {
                    result.Add(cleanedPropertyName, nestedPropertyTitle);
                }
            }
        }
    }

    private string CleanPropertyName(string propertyName)
    {
        var cleanedName = propertyName
            .Replace("\r\n", "")
            .Replace("\n", "")   
            .Replace("\r", "")    
            .Replace("\"", "")  
            .Trim();            

        cleanedName = Regex.Replace(cleanedName, @"\s+", " "); 

        cleanedName = cleanedName.Replace("[[ ", "[[")
                                 .Replace(" ]]", "]]")
                                 .Replace(", ", ",");

        return cleanedName;
    }
}
