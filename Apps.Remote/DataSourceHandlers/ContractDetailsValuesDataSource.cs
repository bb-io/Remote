using Apps.Remote.Api;
using Apps.Remote.Invocables;
using Apps.Remote.Models.Dtos;
using Apps.Remote.Models.Requests.Employments;
using Apps.Remote.Models.Responses.Schemas;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using Newtonsoft.Json;
using RestSharp;

namespace Apps.Remote.DataSourceHandlers;

public class ContractDetailsValuesDataSource(
    InvocationContext invocationContext,
    [ActionParameter] UpdateEmploymentRequest updateRequest)
    : AppInvocable(invocationContext), IAsyncDataSourceHandler
{
    public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(updateRequest.CountryCode))
        {
            throw new InvalidOperationException("You should provide a country code first");
        }

        var request = new ApiRequest($"/v1/countries/{updateRequest.CountryCode}/contract_details", Method.Get, Creds);
        var response = await Client.ExecuteWithErrorHandling<BaseDto<FormSchemaResponse>>(request);

        var properties = response.Data?.Properties;
        if (properties != null)
        {
            var lastKey = updateRequest.ContractDetailsKeys?.LastOrDefault();
            if (lastKey == null)
            {
                throw new InvalidOperationException("Contract details keys must not be null");
            }

            var key = lastKey.Substring(lastKey.LastIndexOf(']') + 1);
            var property = properties[key];

            if (property == null)
            {
                throw new InvalidOperationException($"Property {key} not found in the schema");
            }

            var jOneOf = property["oneOf"]
                         ?? throw new InvalidOperationException("This property does not have dropdown values");
            var values = JsonConvert.DeserializeObject<List<OneOfDto>>(jOneOf.ToString())!;

            return values.Where(x =>
                    context.SearchString == null ||
                    x.Title.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
                .ToDictionary(x => x.Constant, x => x.Title);
        }

        throw new InvalidOperationException("Properties not found in the schema");
    }
}