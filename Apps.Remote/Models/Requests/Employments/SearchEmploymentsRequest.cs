using Blackbird.Applications.Sdk.Common;

namespace Apps.Remote.Models.Requests.Employments;

public class SearchEmploymentsRequest
{
    [Display("Company ID")]
    public string? CompanyId { get; set; }

    public string? Email { get; set; }

    public string? Status { get; set; }
}