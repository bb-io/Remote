using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.Remote.Models.Responses.Employments;

public class EmploymentResponse
{
    [Display("Employment ID"), JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    [JsonProperty("status")]
    public string Status { get; set; } = string.Empty;

    [JsonProperty("type")]
    public string Type { get; set; } = string.Empty;

    [JsonProperty("country")]
    public CountryResponse Country { get; set; } = new();

    [Display("Full name"), JsonProperty("full_name")]
    public string FullName { get; set; } = string.Empty;

    [Display("Job title"), JsonProperty("job_title")]
    public string JobTitle { get; set; } = string.Empty;

    [Display("Personal email"), JsonProperty("personal_email")]
    public string PersonalEmail { get; set; } = string.Empty;

    [Display("Department ID"), JsonProperty("department_id")]
    public string DepartmentId { get; set; } = string.Empty;
}