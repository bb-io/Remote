using Blackbird.Applications.Sdk.Common;

namespace Apps.Remote.Models.Responses;

public class CurrencyResponse
{
    [Display("Currency code")]
    public string Code { get; set; }

    [Display("Currency name")]
    public string Name { get; set; }

    [Display("Currency symbol")]
    public string Symbol { get; set; }
}