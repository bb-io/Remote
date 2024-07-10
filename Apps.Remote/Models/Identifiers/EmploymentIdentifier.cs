using Apps.Remote.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Remote.Models.Identifiers;

public class EmploymentIdentifier
{
    [Display("Employment ID"), DataSource(typeof(EmploymentDataSource))]
    public string EmploymentId { get; set; } = string.Empty;
}