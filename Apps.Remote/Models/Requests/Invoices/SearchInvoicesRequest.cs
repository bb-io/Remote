using Apps.Remote.DataSourceHandlers;
using Apps.Remote.DataSourceHandlers.Static;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Newtonsoft.Json;

namespace Apps.Remote.Models.Requests.Invoices;

public class SearchInvoicesRequest
{
    [Display("Status"), JsonProperty("status"), StaticDataSource(typeof(InvoiceStatusDataSource))]
    public string? Status { get; set; }

    [Display("Contractor invoice schedule ID"), JsonProperty("contractor_invoice_schedule_id"), DataSource(typeof(InvoiceScheduleDataSource))]
    public string? ContractorInvoiceScheduleId { get; set; }

    [Display("Date from"), JsonProperty("date_from")]
    public DateTime? DateFrom { get; set; }

    [Display("Date to"), JsonProperty("date_to")]
    public DateTime? DateTo { get; set; }

    [Display("Due date from"), JsonProperty("due_date_from")]
    public DateTime? DueDateFrom { get; set; }

    [Display("Due date to"), JsonProperty("due_date_to")]
    public DateTime? DueDateTo { get; set; }

    [Display("Approved date from", Description = "Filters contractor invoices by approved date greater than or equal to the value"), JsonProperty("approved_date_from")]
    public DateTime? ApprovedDateFrom { get; set; }

    [Display("Approved date to", Description = "Filters contractor invoices by approved date less than or equal to the value."), JsonProperty("approved_date_to")]
    public DateTime? ApprovedDateTo { get; set; }

    [Display("Paid out date from"), JsonProperty("paid_out_date_from")]
    public DateTime? PaidOutDateFrom { get; set; }

    [Display("Paid out date to"), JsonProperty("paid_out_date_to")]
    public DateTime? PaidOutDateTo { get; set; }
}