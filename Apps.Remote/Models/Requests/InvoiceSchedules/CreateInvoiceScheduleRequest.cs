using Blackbird.Applications.Sdk.Common;

namespace Apps.Remote.Models.Requests.InvoiceSchedules;

public class CreateInvoiceScheduleRequest
{
    [Display("Employment ID")]
    public string EmploymentId { get; set; } = string.Empty;
    
    public string Currency { get; set; } = string.Empty;
    
    [Display("Start date")]
    public DateTime StartDate { get; set; }
    
    public string Periodicity { get; set; } = string.Empty;

    public List<double> Amounts { get; set; } = new();
    
    public List<string> Descriptions { get; set; } = new();
    
    public string? Note { get; set; }
    
    public int? NrOccurrences { get; set; }
    
    public string? Number { get; set; }
}