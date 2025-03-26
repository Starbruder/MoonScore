namespace MoonScore.Services;

public sealed class DateTimeService : IService
{
    public static readonly DateOnly CurrentDate = DateOnly.FromDateTime(DateTime.Now);

    public static string GetFormattedCurrentDate()
        => $"{CurrentDate.Year}-{CurrentDate.Month}-{CurrentDate.Day}";
}
