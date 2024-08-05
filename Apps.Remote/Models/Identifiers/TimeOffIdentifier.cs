using Apps.Remote.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Remote.Models.Identifiers;

public class TimeOffIdentifier
{
    [Display("Time off ID")]
    [DataSource(typeof(TimeOffDataSourceHandler))]
    public string TimeOffId { get; set; }
}