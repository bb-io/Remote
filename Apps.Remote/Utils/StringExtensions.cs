namespace Apps.Remote.Utils;

public static class StringExtensions
{
    public static string ToRemoteNumberFormat(string invoiceNumber)
    {
        return invoiceNumber.Replace("-", string.Empty);
    }
}