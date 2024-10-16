using Apps.Remote.Api;
using Apps.Remote.Invocables;
using Apps.Remote.Models.Dtos;
using Apps.Remote.Models.Identifiers;
using Apps.Remote.Models.Requests.Employments;
using Apps.Remote.Models.Responses.CustomFields;
using Apps.Remote.Models.Responses.Employments;
using Apps.Remote.Models.Responses.Schemas;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace Apps.Remote.Actions;

[ActionList]
public class EmploymentActions(InvocationContext invocationContext) : AppInvocable(invocationContext)
{
    private const int PageSize = 50;

    [Action("Search employments", Description = "Search employments based on specified criteria")]
    public async Task<EmploymentsResponse> SearchEmployments([ActionParameter] SearchEmploymentsRequest request)
    {
        var allEmployments = new List<EmploymentResponse>();
        var currentPage = 1;
        EmploymentsResponse employmentsResponse;

        do
        {
            var apiRequest = CreateApiRequest(request, currentPage, PageSize);
            var response = await Client.ExecuteWithErrorHandling<BaseDto<EmploymentsResponse>>(apiRequest);
            employmentsResponse = response.Data ?? new EmploymentsResponse();

            if (employmentsResponse.Employments != null)
            {
                allEmployments.AddRange(employmentsResponse.Employments);
            }

            currentPage++;
        } while (currentPage <= employmentsResponse.TotalPages);

        return new EmploymentsResponse
        {
            TotalCount = allEmployments.Count,
            CurrentPage = 1,
            TotalPages = 1,
            Employments = allEmployments
        };
    }

    [Action("Get employment", Description = "Get employment by ID")]
    public async Task<EmploymentResponse> GetEmployment([ActionParameter] EmploymentIdentifier identifier)
    {
        var apiRequest = new ApiRequest($"/v1/employments/{identifier.EmploymentId}", Method.Get, Creds);
        var response = await Client.ExecuteWithErrorHandling<BaseDto<EmploymentDto>>(apiRequest);
        response.Data?.Employment.SetContractDetails();

        return response.Data?.Employment!;
    }

    [Action("Get employment custom field", Description = "Get employment custom field value by ID")]
    public async Task<CustomFieldValueResponse> GetEmploymentCustomFieldValue(
        [ActionParameter] CustomFieldIdentifier customFieldIdentifier,
        [ActionParameter] EmploymentIdentifier employmentIdentifier)
    {
        var endpoint =
            $"/v1/custom-fields/{customFieldIdentifier.CustomFieldId}/values/{employmentIdentifier.EmploymentId}";
        var apiRequest = new ApiRequest(endpoint, Method.Get, Creds);
        var response = await Client.ExecuteWithErrorHandling<BaseDto<CustomFieldDto>>(apiRequest);
        if (response.Data != null)
        {
            response.Data.CustomFieldValue.Value ??= string.Empty;
        }

        return response.Data?.CustomFieldValue ?? new CustomFieldValueResponse()
            { CustomFieldId = customFieldIdentifier.CustomFieldId, Value = string.Empty };
    }

    [Action("Create employment", Description = "Create employment with specified data")]
    public async Task<EmploymentResponse> CreateEmployment([ActionParameter] CreateEmploymentRequest request)
    {
        var hasSeniorityDate = request.SeniorityDate.HasValue;
        var body = new Dictionary<string, object>()
        {
            { "country_code", request.CountryCode },
            { "type", string.IsNullOrEmpty(request.Type) ? "employee" : request.Type }
        };
        
        if(!string.IsNullOrEmpty(request.ExternalId))
        {
            body.Add("external_id", request.ExternalId);
        }

        if (!string.IsNullOrEmpty(request.CompanyId))
        {
            body.Add("company_id", request.CompanyId);
        }

        var basicInformation = new Dictionary<string, object>
        {
            { "email", request.Email },
            { "job_title", request.JobTitle },
            { "name", request.Name },
            { "provisional_start_date", request.ProvisionalStartDate.ToString("yyyy-MM-dd") },
            { "has_seniority_date", hasSeniorityDate ? "yes" : "no" }
        };

        if (hasSeniorityDate)
        {
            basicInformation.Add("seniority_date", request.SeniorityDate!.Value.ToString("yyyy-MM-dd"));
        }

        body.Add("basic_information", basicInformation);

        var apiRequest = new ApiRequest("/v1/employments", Method.Post, Creds)
            .WithJsonBody(body);

        var response = await Client.ExecuteWithErrorHandling<BaseDto<EmploymentDto>>(apiRequest);

        await Task.Delay(1500);
        return await GetEmployment(new EmploymentIdentifier { EmploymentId = response.Data?.Employment!.Id! });
    }

    [Action("Update employment", Description = "Update employment by ID with specified data")]
    public async Task<EmploymentResponse> UpdateEmployment(
        [ActionParameter] UpdateEmploymentRequest updateEmploymentRequest)
    {
        var body = new Dictionary<string, object>();

        if (!string.IsNullOrEmpty(updateEmploymentRequest.FullName))
        {
            body.Add("full_name", updateEmploymentRequest.FullName);
        }

        if (!string.IsNullOrEmpty(updateEmploymentRequest.JobTitle))
        {
            body.Add("job_title", updateEmploymentRequest.JobTitle);
        }

        if (!string.IsNullOrEmpty(updateEmploymentRequest.PersonalEmail))
        {
            body.Add("personal_email", updateEmploymentRequest.PersonalEmail);
        }

        if (!string.IsNullOrEmpty(updateEmploymentRequest.PricingPlanFrequency))
        {
            body.Add("pricing_plan_details", new
            {
                frequency = updateEmploymentRequest.PricingPlanFrequency
            });
        }

        if (updateEmploymentRequest.ShouldChangeContractDetails())
        {
            var employment = await GetEmployment(new EmploymentIdentifier
                { EmploymentId = updateEmploymentRequest.EmploymentId });
            var contractDetails = employment.ContractDetails ?? new JObject();
            var mergedContractDetails = updateEmploymentRequest.MergeContractDetails();

            var contractDetailsRequest =
                new ApiRequest($"/v1/countries/{updateEmploymentRequest.CountryCode}/contract_details", Method.Get,
                    Creds);
            var contractDetailsResponse =
                await Client.ExecuteWithErrorHandling<BaseDto<FormSchemaResponse>>(contractDetailsRequest);
            var properties = contractDetailsResponse.Data?.Properties;

            var result = new Dictionary<string, object>();
            if (properties != null)
            {
                foreach (var property in properties)
                {
                    var matchingItems = mergedContractDetails
                        .Where(x => x.Key.Contains(property
                            .Key))
                        .ToList();

                    foreach (var item in matchingItems)
                    {
                        var type = item.Key.Split(']')[0].Substring(1);
                        var key = item.Key.Substring(item.Key.LastIndexOf(']') + 1);

                        var nested = key.Contains('.');
                        if (nested)
                        {
                            var rootKey = key.Split('.')[0];
                            var nestedKey = key.Split('.')[1];

                            if (!result.ContainsKey(rootKey))
                            {
                                result[rootKey] = new JObject();
                            }

                            var innerJObject = (JObject)result[rootKey];
                            innerJObject[nestedKey] = type.Contains("integer") || type.Contains("number")
                                ? Convert.ToInt32(mergedContractDetails[item.Key])
                                : mergedContractDetails[item.Key];
                        }
                        else
                        {
                            result[key] = type.Contains("integer") || type.Contains("number")
                                ? Convert.ToInt32(mergedContractDetails[item.Key])
                                : mergedContractDetails[item.Key];
                        }
                    }

                    if (!result.ContainsKey(property.Key))
                    {
                        var existingValue = contractDetails[property.Key];
                        if (existingValue != null)
                        {
                            result.Add(property.Key, existingValue);
                        }
                    }
                }
            }

            body.Add("contract_details", result);
        }
        
        var apiRequest = new ApiRequest($"/v1/employments/{updateEmploymentRequest.EmploymentId}", Method.Patch, Creds)
            .WithJsonBody(body);

        var response = await Client.ExecuteWithErrorHandling<BaseDto<EmploymentDto>>(apiRequest);
        response.Data?.Employment.SetContractDetails();
        return response.Data?.Employment!;
    }

    [Action("Invite employment", Description = "Invite employment by ID to start the self-enrollment")]
    public async Task InviteEmployment([ActionParameter] EmploymentIdentifier identifier)
    {
        var apiRequest = new ApiRequest($"/v1/employments/{identifier.EmploymentId}/invite", Method.Post, Creds);
        await Client.ExecuteWithErrorHandling(apiRequest);
    }

    private ApiRequest CreateApiRequest(SearchEmploymentsRequest request, int currentPage, int pageSize)
    {
        var apiRequest = new ApiRequest("/v1/employments", Method.Get, Creds);

        if (!string.IsNullOrEmpty(request.CompanyId))
        {
            apiRequest.AddParameter("company_id", request.CompanyId, ParameterType.QueryString);
        }

        if (!string.IsNullOrEmpty(request.Email))
        {
            apiRequest.AddParameter("email", request.Email, ParameterType.QueryString);
        }

        if (!string.IsNullOrEmpty(request.Status))
        {
            apiRequest.AddParameter("status", request.Status, ParameterType.QueryString);
        }

        apiRequest.AddParameter("page", currentPage, ParameterType.QueryString);
        apiRequest.AddParameter("page_size", pageSize, ParameterType.QueryString);

        return apiRequest;
    }
}