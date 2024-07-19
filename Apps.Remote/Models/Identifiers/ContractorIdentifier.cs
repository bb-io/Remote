using Apps.Remote.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Remote.Models.Identifiers;

public class ContractorIdentifier
{
    [Display("Contractor ID", Description = "This field is only for contractors. It is required to specify contractor employment ID."), DataSource(typeof(ContractorDataSource))]
    public string EmploymentId { get; set; } = string.Empty;
}