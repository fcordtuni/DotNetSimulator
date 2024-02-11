//Author: Elisabeth Gisser

namespace ModBusHistorian.DataHandling;

internal static class DateTimeExtensions
{
    public static long ToUnixEpochInMilliSecondsTime(this DateTime dateTime)
    {
        if (dateTime == DateTime.MinValue)
            return 0;
        var dateTime1 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var timeSpan = dateTime - dateTime1;
        return timeSpan.TotalMilliseconds >= 0.0 ? (long)timeSpan.TotalMilliseconds : throw new ArgumentOutOfRangeException(nameof(dateTime));
    }
}