using Blackbird.Applications.Sdk.Common;

namespace Apps.Remote.Models.Responses.Expenses;

public class ReceiptResponse
{
    [Display("Receipt ID")]
    public string Id { get; set; }

    [Display("Inserted at")]
    public DateTime InsertedAt { get; set; }

    [Display("Receipt name")]
    public string Name { get; set; }

    [Display("Sub type")]
    public string SubType { get; set; }

    [Display("Type")]
    public string Type { get; set; }
}