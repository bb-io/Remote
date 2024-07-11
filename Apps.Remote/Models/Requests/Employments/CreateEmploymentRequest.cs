using Apps.Remote.DataSourceHandlers;
using Apps.Remote.DataSourceHandlers.Static;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Remote.Models.Requests.Employments;

public class CreateEmploymentRequest
{
    [Display("Email")]
    public string Email { get; set; } = string.Empty;
    
    [Display("Job title")]
    public string JobTitle { get; set; } = string.Empty;
    
    [Display("Name")]
    public string Name { get; set; } = string.Empty;
    
    [Display("Provisional start date", Description = "Indicates the expected start date of the employee or contractor")]
    public DateTime ProvisionalStartDate { get; set; }

    [Display("Seniority date", Description = "The date the employee first started working for your company. If you don’t include a seniority date, the employee’s start date with Remote will be deemed as the start of the employee’s seniority")]
    public DateTime? SeniorityDate { get; set; }
    
    [Display("Country code"), DataSource(typeof(CountryDataSource))]
    public string CountryCode { get; set; } = string.Empty;
    
    [Display("Type", Description = "If not provided, it will default to 'employee'"), StaticDataSource(typeof(EmploymentTypeDataSource))]
    public string? Type { get; set; } = string.Empty;
    
    [Display("Company ID")]
    public string? CompanyId { get; set; } = string.Empty;
}