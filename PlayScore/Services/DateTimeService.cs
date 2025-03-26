namespace MoonScore.Services;

public sealed class DateTimeService : IService
{
    public static readonly DateOnly CurrentDate = DateOnly.FromDateTime(DateTime.Now);

    private static void FormatDayOrMonth(int dateNumber, Span<char> buffer)
    {
        // If the number is less than 10, put a leading zero in the buffer.
        if (dateNumber < 10)
        {
            buffer[0] = '0';
            buffer[1] = (char)('0' + dateNumber);
            return;
        }

        buffer[0] = (char)('0' + (dateNumber / 10));
        buffer[1] = (char)('0' + (dateNumber % 10));
    }

    public static string GetFormattedCurrentDate()
    {
        Span<char> result = stackalloc char[10];

        // Format the year as 4 digits
        CurrentDate.Year.TryFormat(result[..4], out int _);

        // Insert the dash between year and month
        result[4] = '-';

        // Format the month into 2 digits
        Span<char> monthBuffer = stackalloc char[2];
        FormatDayOrMonth(CurrentDate.Month, monthBuffer);
        monthBuffer.CopyTo(result.Slice(5, 2));

        // Insert the dash between month and day
        result[7] = '-';

        // Format the day into 2 digits
        Span<char> dayBuffer = stackalloc char[2];
        FormatDayOrMonth(CurrentDate.Day, dayBuffer);
        dayBuffer.CopyTo(result.Slice(8, 2));

        return new string(result);
    }


    //private static string GetFormattedDayOrMonth(int dateNumber)
    //{
    //    return dateNumber < 10
    //      ? $"0{dateNumber}"
    //      : dateNumber.ToString();
    //}

    //public static string GetFormattedCurrentDate()
    //{
    //    // Format the date as "YYYY-MM-DD"
    //    var day = GetFormattedDayOrMonth(CurrentDate.Day);
    //    var month = GetFormattedDayOrMonth(CurrentDate.Month);

    //    return $"{CurrentDate.Year}-{month}-{day}";
    //}
}
