using Newtonsoft.Json;

namespace Apps.Remote.Models.Dtos;

public class BlackbirdInvoiceDto
{
    [JsonProperty("invoices")]
    public List<BlackbirdInvoice> Invoices { get; set; } = new();
}

public class BlackbirdInvoice
{
    [JsonProperty("customer_name")]
    public string CustomerName { get; set; } = string.Empty;

    [JsonProperty("invoice_number")]
    public string InvoiceNumber { get; set; } = string.Empty;

    [JsonProperty("invoice_date")]
    public DateTime InvoiceDate { get; set; }

    [JsonProperty("currency")]
    public string Currency { get; set; } = string.Empty;

    [JsonProperty("lines")]
    public List<BlackbirdInvoiceLine> Lines { get; set; } = new();

    [JsonProperty("sub_total")]
    public double SubTotal { get; set; }

    [JsonProperty("taxes")]
    public List<BlackbirdTax> Taxes { get; set; } = new();

    [JsonProperty("total")]
    public double Total { get; set; }

    [JsonProperty("custom_fields")]
    public Dictionary<string, object> CustomFields { get; set; } = new();
}

public class BlackbirdInvoiceLine
{
    [JsonProperty("description")]
    public string Description { get; set; } = string.Empty;

    [JsonProperty("quantity")]
    public int Quantity { get; set; }

    [JsonProperty("unit_price")]
    public double UnitPrice { get; set; }

    [JsonProperty("amount")]
    public double Amount { get; set; }
}

public class BlackbirdTax
{
    [JsonProperty("description")]
    public string Description { get; set; } = string.Empty;

    [JsonProperty("amount")]
    public double Amount { get; set; }
}