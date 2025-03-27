using System.Globalization;

namespace MoonScore.Services;

public sealed class DateTimeService : IService
{
    public static readonly DateOnly CurrentDate = DateOnly.FromDateTime(DateTime.Now);

    private static void FormatDayOrMonth(int dateNumber, Span<char> buffer)
    {
        const char charToAppend = '0';

        // If the number is less than 10, put a leading zero in the buffer (charToAppend).
        if (dateNumber < 10)
        {
            buffer[0] = charToAppend;
            buffer[1] = (char)(charToAppend + dateNumber);
            return;
        }

        buffer[0] = (char)(charToAppend + (dateNumber / 10));
        buffer[1] = (char)(charToAppend + (dateNumber % 10));
    }

    public static string GetFormattedCurrentDate()
    {
        Span<char> result = stackalloc char[10];
        const char seperatorChar = '-';

        // Format the year as 4 digits
        CurrentDate.Year.TryFormat(result[..4], out int _);

        // Insert the dash between year and month (seperatorChar).
        result[4] = seperatorChar;

        // Format the month into 2 digits
        Span<char> monthBuffer = stackalloc char[2];
        FormatDayOrMonth(CurrentDate.Month, monthBuffer);
        monthBuffer.CopyTo(result.Slice(5, 2));

        // Insert the dash between month and day (seperatorChar).
        result[7] = seperatorChar;

        // Format the day into 2 digits
        Span<char> dayBuffer = stackalloc char[2];
        FormatDayOrMonth(CurrentDate.Day, dayBuffer);
        dayBuffer.CopyTo(result.Slice(8, 2));

        return new string(result);
    }

    public static string FormatDateInput(string date)
    {
        var parsedDate = DateTime.ParseExact(date, "dd.MM.yyyy", CultureInfo.InvariantCulture);
        return parsedDate.ToString("yyyy-MM-dd");
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
