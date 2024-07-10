using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Remote.DataSourceHandlers.Static;

public class InvoiceStatusDataSource : IStaticDataSourceHandler
{
    public Dictionary<string, string> GetData()
    {
        return new Dictionary<string, string>
        {
            { "issued", "Issued" },
            { "approved", "Approved" },
            { "pending_payment", "Pending payment" },
            { "externally_paid", "Externally paid" },
            { "rejected", "Rejected" },
            { "blocked", "Blocked" },
            { "enqueued", "Enqueued" },
            { "processing", "Processing" },
            { "manual_payout", "Manual payout" },
            { "paid_out", "Paid out" },
            { "pay_out_failed", "Pay out failed" },
            { "funds_returned", "Funds returned" }
        };
    }
}