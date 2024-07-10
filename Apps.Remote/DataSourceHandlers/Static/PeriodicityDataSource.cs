using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Remote.DataSourceHandlers.Static;

public class PeriodicityDataSource : IStaticDataSourceHandler
{
    public Dictionary<string, string> GetData()
    {
        return new Dictionary<string, string>
        {
            { "bi_weekly", "Bi-weekly" },
            { "monthly", "Monthly" },
            { "semi_monthly", "Semi-monthly" },
            { "weekly", "Weekly" }
        };
    }
}