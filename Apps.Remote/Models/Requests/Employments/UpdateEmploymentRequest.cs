using Apps.Remote.Models.Identifiers;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Remote.Models.Requests.Employments;

public class UpdateEmploymentRequest : EmploymentIdentifier
{
    [Display("Full name")] 
    public string? FullName { get; set; }

    [Display("Job title")]
    public string? JobTitle { get; set; }
    
    [Display("Personal email")]
    public string? PersonalEmail { get; set; }
}