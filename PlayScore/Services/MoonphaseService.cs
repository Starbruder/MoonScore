using Newtonsoft.Json;
using MoonScore.Models;
using System.Configuration;
using System.Globalization;
using System.Net.Http;
using MoonScore.DataConstants;

namespace MoonScore.Services;

public sealed class MoonphaseService
{
    private static readonly HttpClient _httpClient = new();
    private readonly string apiKey = ConfigurationManager.AppSettings["API_KEY_MOON"] ?? string.Empty;
    private readonly string ApiUrl;

    private MoonPhaseModel? _cachedMoonPhase;
    private DateOnly _cachedMoonPhaseDate = DateOnly.MinValue;

    public MoonphaseService()
        => ApiUrl = $"https://api.ipgeolocation.io/astronomy?apiKey={apiKey}&date=";

    public async Task<MoonPhaseModel?> GetMoonPhaseAsync(string date, double latitude, double longitude)
    {
        try
        {
            string latitudeStr = latitude.ToString(CultureInfo.InvariantCulture);  
            string longitudeStr = longitude.ToString(CultureInfo.InvariantCulture);

            string requestUrl = $"{ApiUrl}{date}&lat={latitudeStr}&long={longitudeStr}";

            var httpResponseMessage = await _httpClient.GetAsync(requestUrl);
            httpResponseMessage.EnsureSuccessStatusCode();

            string content = await httpResponseMessage.Content.ReadAsStringAsync();

            var moonphaseData = JsonConvert.DeserializeObject<MoonPhaseModel>(content);
            return moonphaseData;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return null;
        }
    }

    public async Task<MoonPhaseModel?> GetCachedRostockMoonPhaseTodayAsync(string date)
    {
        try
        {
            if (_cachedMoonPhaseDate == DateTimeService.CurrentDate)
            {
                return _cachedMoonPhase;
            }

            var moonPhase = await GetMoonPhaseAsync(date, RostockData.latitude, RostockData.longitude);

            if (moonPhase is not null)
            {
                CacheMoonPhaseAndUpdateCachedDate(moonPhase);
            }

            return moonPhase;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return null;
        }
    }

    private void CacheMoonPhaseAndUpdateCachedDate(MoonPhaseModel moonPhase)
    {
        _cachedMoonPhase = moonPhase;
        _cachedMoonPhaseDate = DateTimeService.CurrentDate;
    }
}
