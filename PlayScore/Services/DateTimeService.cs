namespace MoonScore.Services;

public sealed class DateTimeService : IService
{
    public static readonly DateOnly CurrentDate = DateOnly.FromDateTime(DateTime.Now);

    private static string GetFormattedDayOrMonth(int dateNumber)
    {
        var dateNumberString = dateNumber.ToString();
        return dateNumberString.Length < 2
            ? "0" + dateNumberString
            : dateNumberString;
    }

    public static string GetFormattedCurrentDate()
    {
        var day = GetFormattedDayOrMonth(CurrentDate.Day);
        var month = GetFormattedDayOrMonth(CurrentDate.Month);

        return $"{CurrentDate.Year}-{month}-{day}";
    }
}
