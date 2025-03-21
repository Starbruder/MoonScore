namespace MoonScore.Services;

public sealed class MoonphaseTranslationService : IService
{
    private readonly Dictionary<string, string> _moonPhaseTranslations = new()
    {
        { "NEW_MOON", "Neumond" },
        { "WAXING_CRESCENT", "Zunehmender Halbmond" },
        { "FIRST_QUARTER", "Erstes Viertel" },
        { "WAXING_GIBBOUS", "Zunehmender Dreiviertelmond" },
        { "FULL_MOON", "Vollmond" },
        { "WANING_GIBBOUS", "Abnehmender Dreiviertelmond" },
        { "LAST_QUARTER", "Letztes Viertel" },
        { "WANING_CRESCENT", "Abnehmender Halbmond" }
    };

    public string Translate(string moonPhase)
    {
        return _moonPhaseTranslations.TryGetValue(moonPhase, out var translation)
            ? translation
            : moonPhase; // Return the original name if no translation is found
    }
}
