using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Remote.DataSourceHandlers.Static;

public class YesNoDataSource : IStaticDataSourceHandler
{
    public Dictionary<string, string> GetData()
    {
        return new()
        {
            { "yes", "Yes" },
            { "no", "No" }
        };
    }
}