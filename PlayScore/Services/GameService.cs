using Newtonsoft.Json;
using MoonScore.Models;
using System.Configuration;
using System.Net.Http;
using MoonScore.DataConstants;
using System.Windows;

namespace MoonScore.Services;

public sealed class GameService
{
    private readonly HttpClient _httpClient = new();
    private readonly string apiKey = ConfigurationManager.AppSettings["API_KEY_GAMES"] ?? string.Empty;
    private readonly string ApiUrl;
    private readonly MoonphaseService _moonphaseService;
    private readonly MoonphaseTranslationService _moonphaseTranslationService;

    public GameService(MoonphaseService moonphaseService, MoonphaseTranslationService moonphaseTranslator)
    {
        ApiUrl = $"https://api.rawg.io/api/games?key={apiKey}&dates=";
        _moonphaseService = moonphaseService;
        _moonphaseTranslationService = moonphaseTranslator;
    }

    public async Task<List<GameModel>> GetGamesByReleaseDateAsync(string releaseDate)
    {
        try
        {
            string url = $"{ApiUrl}{releaseDate},{releaseDate}";

            var responseMessage = await _httpClient.GetAsync(url);
            responseMessage.EnsureSuccessStatusCode();

            string content = await responseMessage.Content.ReadAsStringAsync();

            var jsonResponse = JsonConvert.DeserializeObject<dynamic>(content);
            var games = new List<GameModel>();

            if (jsonResponse is null || jsonResponse.results is null)
            {
                MessageBox.Show("Failed to retrieve game data. Please check your connection or try again.");
                return [];
            }

            foreach (var game in jsonResponse.results)
            {
                string gameReleaseDate = game.released;
                double gameRating = game.rating;

                int? moonPhaseId = null;

                if (gameRating > 0)
                {
                    moonPhaseId = await GetGameMoonPhaseIdAsync(gameReleaseDate); 
                }

                games.Add(new()
                {
                    Id = game.id,
                    Name = game.name,
                    Released = game.released,
                    Rating = game.rating,
                    MondphaseID = moonPhaseId ?? 0 
                });
            }

            return games;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching games: {ex.Message}");
            return [];
        }
    }

    public async Task<int?> GetGameMoonPhaseIdAsync(string date)
    {
        var moonPhaseData = await _moonphaseService.GetMoonPhaseAsync(date, RostockData.latitude, RostockData.longitude);
        var (moonPhaseId, _) = _moonphaseTranslationService.GetMoonPhaseData(moonPhaseData.MoonPhase);
        return moonPhaseId; 
    }
}
