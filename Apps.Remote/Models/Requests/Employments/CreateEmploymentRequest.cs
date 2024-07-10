using Apps.Remote.DataSourceHandlers.Static;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Remote.Models.Requests.Employments;

public class CreateEmploymentRequest
{
    [Display("Email")]
    public string Email { get; set; } = string.Empty;

    [Display("Has seniority date")]
    public bool? HasSeniorityDate { get; set; }
    
    [Display("Job title")]
    public string JobTitle { get; set; } = string.Empty;
    
    [Display("Name")]
    public string Name { get; set; } = string.Empty;
    
    [Display("Provisional start date")]
    public DateTime ProvisionalStartDate { get; set; }
    
    [Display("Country code")]
    public string CountryCode { get; set; } = string.Empty;
    
    [Display("Type"), StaticDataSource(typeof(EmploymentTypeDataSource))]
    public string? Type { get; set; } = string.Empty;
    
    [Display("Company ID")]
    public string? CompanyId { get; set; } = string.Empty;
}