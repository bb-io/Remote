using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Remote.DataSourceHandlers.Static;

public class EmploymentTypeDataSource : IStaticDataSourceHandler
{
    public Dictionary<string, string> GetData()
    {
        return new()
        {
            { "employee", "Employee" },
            { "contractor", "Contractor" }
        };
    }
}