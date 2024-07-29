using Apps.Remote.DataSourceHandlers.Static;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Remote.Models.Requests.Employments;

public class SearchEmploymentsRequest
{
    [Display("Company ID")]
    public string? CompanyId { get; set; }

    public string? Email { get; set; }

    [StaticDataSource(typeof(EmploymentStatusDataSource))]
    public string? Status { get; set; }
}