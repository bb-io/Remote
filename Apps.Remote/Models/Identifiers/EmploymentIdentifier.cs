using Blackbird.Applications.Sdk.Common;

namespace Apps.Remote.Models.Identifiers;

public class EmploymentIdentifier
{
    [Display("Employment ID")]
    public string EmploymentId { get; set; } = string.Empty;
}