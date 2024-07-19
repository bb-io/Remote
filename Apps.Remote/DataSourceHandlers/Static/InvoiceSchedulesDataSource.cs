using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Remote.DataSourceHandlers.Static;

public class InvoiceSchedulesDataSource : IStaticDataSourceHandler
{
    public Dictionary<string, string> GetData()
    {
        return new Dictionary<string, string>
        {
            { "inactive", "Inactive" },
            { "completed", "Completed" },
            { "active", "Active" },
            { "processing", "Processing" },
            { "pending_company_action", "Pending company action" },
            { "pending_contractor_action", "Pending contractor action" },
            { "generation_failed_unrelated_to_withdrawal_method", "Generation failed unrelated to withdrawal method" }
        };
    }
}