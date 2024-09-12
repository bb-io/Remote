using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Remote.DataSourceHandlers.Static;

public class PricingPlanFrequencyDataSource : IStaticDataSourceHandler
{
    public Dictionary<string, string> GetData()
    {
        return new()
        {
            { "annually", "Annually" },
            { "monthly", "Monthly" }
        };
    }
}