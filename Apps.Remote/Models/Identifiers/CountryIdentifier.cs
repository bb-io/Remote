using Apps.Remote.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Remote.Models.Identifiers;

public class CountryIdentifier
{
    [Display("Country code"), DataSource(typeof(CountryDataSource))]
    public string CountryCode { get; set; } = string.Empty;
}