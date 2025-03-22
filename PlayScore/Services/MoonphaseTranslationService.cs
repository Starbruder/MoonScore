namespace MoonScore.Services;

public sealed class MoonphaseTranslationService : IService
{
    private readonly Dictionary<string, string> _moonPhaseTranslations = new()
    {
        { "NEW_MOON", "Neumond" },
        { "WAXING_CRESCENT", "Zunehmende Mondsichel" },
        { "FIRST_QUARTER", "Erstes Viertel" },
        { "WAXING_GIBBOUS", "Zunehmender Mond" },
        { "FULL_MOON", "Vollmond" },
        { "WANING_GIBBOUS", "Abnehmender Mond" },
        { "LAST_QUARTER", "Letztes Viertel" },
        { "WANING_CRESCENT", "Abnehmende Mondsichel" }
    };

    public string Translate(string moonPhase)
    {
        return _moonPhaseTranslations.TryGetValue(moonPhase, out var translation)
            ? translation
            : moonPhase; // Return the original name if no translation is found
    }
}
