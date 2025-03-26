using MoonScore.Enums;

namespace MoonScore.Services;

public sealed class MoonphaseTranslationService : IService
{
    private readonly Dictionary<string, (int Id, string GermanName)> _moonPhaseData = new()
    {
        { "NEW_MOON",        ( (int)Moonphases.Neumond,              "Neumond") },
        { "WAXING_CRESCENT", ( (int)Moonphases.ZunehmendeSichel, "Zunehmende Sichel") },
        { "FIRST_QUARTER",   ( (int)Moonphases.ErstesViertel,        "Erstes Viertel") },
        { "WAXING_GIBBOUS",  ( (int)Moonphases.ZunehmenderMond,      "Zunehmender Mond") },
        { "FULL_MOON",       ( (int)Moonphases.Vollmond,             "Vollmond") },
        { "WANING_GIBBOUS",  ( (int)Moonphases.AbnehmenderMond,      "Abnehmender Mond") },
        { "LAST_QUARTER",    ( (int)Moonphases.LetztesViertel,       "Letztes Viertel") },
        { "WANING_CRESCENT", ( (int)Moonphases.AbnehmendeSichel, "Abnehmende Sichel") }
    };

    public (int? Id, string? Name) GetMoonPhaseData(string moonPhase)
    {
        return _moonPhaseData.TryGetValue(moonPhase, out var data) ? data : (null, null);
    }
}
