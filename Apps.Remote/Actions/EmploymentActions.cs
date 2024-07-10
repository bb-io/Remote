using Apps.Remote.Api;
using Apps.Remote.Invocables;
using Apps.Remote.Models.Dtos;
using Apps.Remote.Models.Identifiers;
using Apps.Remote.Models.Requests.Employments;
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
    
    [Action("Create employment", Description = "Create employment with specified data")]
    public async Task<EmploymentResponse> CreateEmployment([ActionParameter] CreateEmploymentRequest employmentDto)
    {
        var hasSeniorityDate = employmentDto.HasSeniorityDate ?? false;
        var apiRequest = new ApiRequest("/v1/employments", Method.Post, Creds)
            .WithJsonBody(new
            {
                basic_information = new
                {
                    email = employmentDto.Email,
                    has_seniority_date = hasSeniorityDate ? "yes" : "no",
                    job_title = employmentDto.JobTitle,
                    name = employmentDto.Name,
                    provisional_start_date = employmentDto.ProvisionalStartDate
                },
                country_code = employmentDto.CountryCode,
                company_id = employmentDto.CompanyId,
                type = employmentDto.Type
            });
        
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
    
    [Action("Invite employment", Description = "Invite employment by ID")]
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
            apiRequest.AddParameter("company_id", request.CompanyId);
        }

        if (!string.IsNullOrEmpty(request.Email))
        {
            apiRequest.AddParameter("email", request.Email);
        }

        if (!string.IsNullOrEmpty(request.Status))
        {
            apiRequest.AddParameter("status", request.Status);
        }

        apiRequest.AddParameter("page", currentPage);
        apiRequest.AddParameter("page_size", pageSize);

        return apiRequest;
    }
}