namespace Infrastructure.Extensions;

public static class DateTimeExtensions
{
    public static bool IsTheSameDate(this DateTime srcDate, DateTime desDate)
    {
        return srcDate.Year == desDate.Year && srcDate.Month == desDate.Month && srcDate.Day == desDate.Day;
    }

    public static uint DateTimeToUInt32(this DateTime dateTime)
    {
        DateTime startTime = new(1970, 1, 1, 0, 0, 0, 0);
        TimeSpan currTime = dateTime - startTime;
        uint time_t = Convert.ToUInt32(Math.Abs(currTime.TotalSeconds));
        return time_t;
    }

    public static DateTime TimeStampToDate(this long timeStamp)
    {
        timeStamp = timeStamp < 0 ? 0 : timeStamp;
        return TimeStampToDateTime(timeStamp).Date;
    }

    public static DateTime TimeStampToDateTime(this long timeStamp)
    {
        timeStamp = timeStamp < 0 ? 0 : timeStamp;
        return DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(timeStamp)).DateTime;
    }

    public static long GetTimeStamp(this DateTime dateTime, bool includedTimeValue = false)
    {
        return (long)Math.Round((includedTimeValue ? dateTime : dateTime.Date).Subtract(new DateTime(1970, 1, 1)).TotalSeconds, 0);
    }

    public static long GetFirstDayOfMonthTimeStamp(this DateTime dateTime, bool includeTimeValue = false)
    {
        DateTime dateTime2 = new(dateTime.Year, dateTime.Month, 1);
        return dateTime2.GetTimeStamp(includeTimeValue);
    }

    public static DateTimeOffset GetTimeZoneExpiryDate(DateTimeOffset dateTimeOffset, int zoneHour)
    {
        return new DateTimeOffset(dateTimeOffset.ToOffset(new TimeSpan(zoneHour, 0, 0)).Date, new TimeSpan(zoneHour, 0, 0));
    }

    public static bool GreaterThanWithoutDay(this DateTime dtFrom, DateTime dtTo)
    {
        return dtFrom.Year > dtTo.Year || (dtFrom.Year == dtTo.Year && dtFrom.Month > dtTo.Month);
    }

    public static List<DateRangeInfo> GetDateRangeInfoByMonth(DateTime fromDateTime, DateTime toDateTime)
    {
        List<DateRangeInfo> list = [];
        if (fromDateTime.Date == toDateTime.Date)
        {
            DateRangeInfo item = new()
            {
                FromDate = fromDateTime,
                ToDate = toDateTime
            };
            list.Add(item);
            return list;
        }

        while (toDateTime.AddMonths(1).GreaterThanWithoutDay(fromDateTime))
        {
            DateRangeInfo item2 = new()
            {
                FromDate = list.Count == 0 ? fromDateTime : new DateTime(fromDateTime.Year, fromDateTime.Month, 1),
                ToDate = toDateTime.GreaterThanWithoutDay(fromDateTime) ?
                new DateTime(fromDateTime.Year, fromDateTime.Month, DateTime.DaysInMonth(fromDateTime.Year, fromDateTime.Month)) : toDateTime
            };
            list.Add(item2);
            fromDateTime = fromDateTime.AddMonths(1);
        }

        return list;
    }

    public static int ConverTimestampToYearMonth(long timeStamp)
    {
        DateTime dateTime = TimeStampToDate(timeStamp);
        string text = dateTime.Month < 10 ? "0" + dateTime.Month : dateTime.Month.ToString() ?? "";
        string s = dateTime.Year + text;
        return int.Parse(s);
    }

    public static int ConverTimestampToYearMonthDay(long timeStamp)
    {
        DateTime dateTime = TimeStampToDate(timeStamp);
        string text = dateTime.Month < 10 ? "0" + dateTime.Month : dateTime.Month.ToString() ?? "";
        string text2 = dateTime.Day < 10 ? "0" + dateTime.Day : dateTime.Day.ToString() ?? "";
        string s = dateTime.Year + text + text2;
        return int.Parse(s);
    }

    public static DateTime EndOfDay(this DateTime date)
    {
        return new(date.Year, date.Month, date.Day, 23, 59, 59, 999);
    }

    public static DateTime StartOfDay(this DateTime date)
    {
        return new(date.Year, date.Month, date.Day, 0, 0, 0, 0);
    }

    public static Timestamp ToTimestamp(this DateTime? date)
    {
        DateTime defaultDateTime = new(1970, 01, 01);
        return date is null
            ? Timestamp.FromDateTime(DateTime.SpecifyKind(defaultDateTime, DateTimeKind.Utc))
            : date.Value.ToTimestamp();
    }

    public static Timestamp ToTimestamp(this DateTime dateTime)
    {
        DateTime defaultDateTime = new(1970, 01, 01);
        return dateTime < defaultDateTime
            ? Timestamp.FromDateTime(DateTime.SpecifyKind(defaultDateTime, DateTimeKind.Utc))
            : Timestamp.FromDateTime(DateTime.SpecifyKind(dateTime, DateTimeKind.Utc));
    }

    public static DateTime ToUTC(this DateTime dateTime)
    {
        DateTime dateUnspecified = DateTime.SpecifyKind(dateTime, DateTimeKind.Unspecified);
        return dateUnspecified.ToUniversalTime();
    }
}