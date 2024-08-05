using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Remote.DataSourceHandlers.Static;

public class TimeOffTypeDataHandler : IStaticDataSourceHandler
{
    public Dictionary<string, string> GetData()
    {
        return new()
        {
            { "paid_time_off", "Paid time off" },
            { "sick_leave", "Sick leave" },
            { "public_holiday", "Public holiday" },
            { "unpaid_leave", "Unpaid leave" },
            { "extended_leave", "Extended leave" },
            { "in_lieu_time", "In lieu time" },
            { "maternity_leave", "Maternity leave" },
            { "paternity_leave", "Paternity leave" },
            { "parental_leave", "Parental leave" },
            { "bereavement", "Bereavement leave" },
            { "military_leave", "Military leave" },
            { "other", "Other" }
        };
    }
}