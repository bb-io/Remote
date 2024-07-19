namespace Apps.Remote.Utils;

public static class DateTimeExtensions
{
    public static DateTime ToAppropriateTime(this DateTime dateTime)
    {
        // assuming that server time is Eastern European Summer Time (EEST)
        return dateTime.AddHours(3);
    }
}