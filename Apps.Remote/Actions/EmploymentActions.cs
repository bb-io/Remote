using Apps.Remote.Api;
using Apps.Remote.Invocables;
using Apps.Remote.Models.Dtos;
using Apps.Remote.Models.Identifiers;
using Apps.Remote.Models.Requests.Employments;
using Apps.Remote.Models.Responses.CustomFields;
using Apps.Remote.Models.Responses.Employments;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
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

        return response.Data?.Employment!;
    }
    
    [Action("Get employment custom field", Description = "Get employment custom field value by ID")]
    public async Task<CustomFieldValueResponse> GetEmploymentCustomFieldValue([ActionParameter] CustomFieldIdentifier customFieldIdentifier,
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

        return response.Data?.CustomFieldValue ?? new CustomFieldValueResponse() { CustomFieldId = customFieldIdentifier.CustomFieldId, Value = string.Empty };
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
        return response.Data?.Employment!;
    }
    
    [Action("Update employment", Description = "Update employment by ID with specified data")]
    public async Task<EmploymentResponse> UpdateEmployment([ActionParameter] UpdateEmploymentRequest employmentDto)
    {
        var body = new Dictionary<string, object>();
        
        if (!string.IsNullOrEmpty(employmentDto.FullName))
        {
            body.Add("full_name", employmentDto.FullName);
        }
        
        if (!string.IsNullOrEmpty(employmentDto.JobTitle))
        {
            body.Add("job_title", employmentDto.JobTitle);
        }
        
        if (!string.IsNullOrEmpty(employmentDto.PersonalEmail))
        {
            body.Add("personal_email", employmentDto.PersonalEmail);
        }
        
        var apiRequest = new ApiRequest($"/v1/employments/{employmentDto.EmploymentId}", Method.Patch, Creds)
            .WithJsonBody(body);
        
        var response = await Client.ExecuteWithErrorHandling<BaseDto<EmploymentDto>>(apiRequest);
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