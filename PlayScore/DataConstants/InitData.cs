using MoonScore.Services;

namespace MoonScore.DataConstants;

public static class InitData
{
    public static string GetGamesInitDate() => DateTimeService.GetFormattedCurrentDate();
}
