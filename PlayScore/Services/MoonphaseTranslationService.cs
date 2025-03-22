namespace MoonScore.Services;

public sealed class MoonphaseTranslationService : IService
{
    private readonly Dictionary<string, (int Id, string GermanName)> _moonPhaseData = new()
    {
        { "NEW_MOON", (1, "Neumond") },
        { "WAXING_CRESCENT", (2, "Zunehmende Mondsichel") },
        { "FIRST_QUARTER", (3, "Erstes Viertel") },
        { "WAXING_GIBBOUS", (4, "Zunehmender Mond") },
        { "FULL_MOON", (5, "Vollmond") },
        { "WANING_GIBBOUS", (6, "Abnehmender Mond") },
        { "LAST_QUARTER", (7, "Letztes Viertel") },
        { "WANING_CRESCENT", (8, "Abnehmende Mondsichel") }
    };

    public (int? Id, string? Name) GetMoonPhaseData(string moonPhase)
    {
        return _moonPhaseData.TryGetValue(moonPhase, out var data) ? data : (null, null);
    }
}
