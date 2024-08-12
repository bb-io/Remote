using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Remote.DataSourceHandlers.Static;

public class TimeZoneDataHandler : IStaticDataSourceHandler
{
    public Dictionary<string, string> GetData()
    {
        return new()
        {
            { "UTC", "Coordinated Universal Time" },
            { "America/New_York", "Eastern Time (US & Canada)" },
            { "Europe/London", "British Time" },
            { "Asia/Tokyo", "Japan Standard Time" },
            { "Australia/Sydney", "Australian Eastern Time" },
            { "Europe/Berlin", "Central European Time" },
            { "America/Los_Angeles", "Pacific Time (US & Canada)" }
        };
    }
}