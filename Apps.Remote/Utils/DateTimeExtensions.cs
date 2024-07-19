namespace Apps.Remote.Utils;

public class DateTimeExtensions
{
    public static DateTime ToAppropriateTime(DateTime dateTime)
    {
        var unixTime = ((DateTimeOffset)dateTime).ToUnixTimeSeconds();
        var datetime = DateTimeOffset.FromUnixTimeSeconds(unixTime).UtcDateTime;
        return datetime;
    }
}