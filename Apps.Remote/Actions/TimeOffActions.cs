using Apps.Remote.Api;
using Apps.Remote.Constants;
using Apps.Remote.Invocables;
using Apps.Remote.Models.Dtos;
using Apps.Remote.Models.Identifiers;
using Apps.Remote.Models.Requests.TimeOffs;
using Apps.Remote.Models.Responses.TimeOffs;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using RestSharp;

namespace Apps.Remote.Actions;

[ActionList]
public class TimeOffActions(InvocationContext invocationContext) : AppInvocable(invocationContext)
{
    [Action("Get time off", Description = "Get details of a specific time off")]
    public async Task<TimeOffResponse> GetTimeOff([ActionParameter] TimeOffIdentifier identifier)
    {
        var apiRequest = new ApiRequest($"/v1/timeoff/{identifier.TimeOffId}", Method.Get, Creds);
        var response = await Client.ExecuteWithErrorHandling<BaseDto<TimeOffDto>>(apiRequest);

        return response.Data?.Timeoff!;
    }

    [Action("Create time off", Description = "Create a new time off")]
    public async Task<TimeOffResponse> CreateTimeOff([ActionParameter] CreateTimeOffInput input)
    {
        if (input.StartDate > input.EndDate)
            throw new("End date should be greater than Start date");

        var apiRequest = new ApiRequest("/v1/timeoff", Method.Post, Creds)
            .WithJsonBody(new CreateTimeOffRequest(input), JsonConfig.JsonSettings);
        var response = await Client.ExecuteWithErrorHandling<BaseDto<TimeOffDto>>(apiRequest);

        return response.Data?.Timeoff!;
    }
}