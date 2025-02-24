using Apps.Remote.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Remote.Models.Identifiers;

public class CustomFieldIdentifier
{
    [Display("Custom field ID"), DataSource(typeof(CustomFieldDataSource))] 
    public string CustomFieldId { get; set; } = string.Empty;
}