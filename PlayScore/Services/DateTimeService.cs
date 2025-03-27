using System.Globalization;

namespace MoonScore.Services;

public sealed class DateTimeService : IService
{
    public static readonly DateOnly CurrentDate = DateOnly.FromDateTime(DateTime.Now);

    private static void FormatDayOrMonth(int dateNumber, Span<char> buffer)
    {
        const char charToAppend = '0';

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

        CurrentDate.Year.TryFormat(result[..4], out int _);

        result[4] = seperatorChar;

        Span<char> monthBuffer = stackalloc char[2];
        FormatDayOrMonth(CurrentDate.Month, monthBuffer);
        monthBuffer.CopyTo(result.Slice(5, 2));

        result[7] = seperatorChar;

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
}
