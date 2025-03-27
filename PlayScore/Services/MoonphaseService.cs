using Newtonsoft.Json;
using MoonScore.Models;
using System.Configuration;
using System.Globalization;
using System.Net.Http;
using MoonScore.DataConstants;
using System.Windows;

namespace MoonScore.Services;

public sealed class MoonphaseService
{
    private static readonly HttpClient _httpClient = new();
    private readonly string apiKey = ConfigurationManager.AppSettings["API_KEY_MOON"] ?? string.Empty;
    private readonly string ApiUrl;

    private DateOnly? _cachedDate;
    private MoonPhaseModel? _cachedMoonPhase;

    public MoonphaseService()
        => ApiUrl = $"https://api.ipgeolocation.io/astronomy?apiKey={apiKey}&date=";

    public async Task<MoonPhaseModel?> GetMoonPhaseAsync(string date, double latitude, double longitude)
    {
        if (DateOnly.TryParse(date, out DateOnly parsedDate) && _cachedDate == parsedDate)
        {
            MessageBox.Show($"Skipping API call. Data for {date} is already cached.");
            return _cachedMoonPhase;
        }

        try
        {
            string latitudeStr = latitude.ToString(CultureInfo.InvariantCulture);
            string longitudeStr = longitude.ToString(CultureInfo.InvariantCulture);

            string requestUrl = $"{ApiUrl}{date}&lat={latitudeStr}&long={longitudeStr}";
            var httpResponseMessage = await _httpClient.GetAsync(requestUrl);
            httpResponseMessage.EnsureSuccessStatusCode();

            string content = await httpResponseMessage.Content.ReadAsStringAsync();
            var moonphaseData = JsonConvert.DeserializeObject<MoonPhaseModel>(content);

            if (moonphaseData is not null)
            {
                CacheMoonPhaseAndUpdateCachedDate(moonphaseData, parsedDate);
            }

            return moonphaseData;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return null;
        }
    }

    private void CacheMoonPhaseAndUpdateCachedDate(MoonPhaseModel moonPhase, DateOnly date)
    {
        _cachedMoonPhase = moonPhase;
        _cachedDate = date;
    }
}
