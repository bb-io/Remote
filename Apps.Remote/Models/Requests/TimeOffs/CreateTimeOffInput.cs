using Apps.Remote.DataSourceHandlers;
using Apps.Remote.DataSourceHandlers.Static;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Remote.Models.Requests.TimeOffs;

public class CreateTimeOffInput
{
    [Display("Employment ID"), DataSource(typeof(EmploymentDataSource))]
    public string EmploymentId { get; set; }

    [Display("Start date")] public DateTime StartDate { get; set; }

    [Display("End date")] public DateTime EndDate { get; set; }

    [Display("Time off type"), StaticDataSource(typeof(TimeOffTypeDataHandler))]
    public string TimeoffType { get; set; }

    [StaticDataSource(typeof(TimeZoneDataHandler))]
    public string Timezone { get; set; }

    [Display("Approved at")] public DateTime ApprovedAt { get; set; }

    [Display("Approver ID"), DataSource(typeof(CompanyManagerDataHandler))]
    public string ApproverId { get; set; }
}