using Newtonsoft.Json;
using MoonScore.Models;
using System.Configuration;
using System.Net.Http;
using MoonScore.DataConstants;
using System.Windows;

namespace MoonScore.Services;

public sealed class GameService : IService
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
            // Build the URL with the date filter, first date is start, second is end
            string url = $"{ApiUrl}{releaseDate},{releaseDate}";

            // Send a GET request
            var responseMessage = await _httpClient.GetAsync(url);
            responseMessage.EnsureSuccessStatusCode();

            // Read the response content
            string content = await responseMessage.Content.ReadAsStringAsync();

            // Deserialize the JSON into a list of games
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

                string? moonPhaseName = null;

                if (gameRating > 0)
                {
                    moonPhaseName = await GetGameMoonPhaseAsync(gameReleaseDate);
                }

                games.Add(new()
                {
                    Id = game.id,
                    Name = game.name,
                    Released = game.released,
                    Rating = game.rating,
                    MondphaseName = moonPhaseName
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

    public async Task<string> GetGameMoonPhaseAsync(string date)
    {
        var moonPhaseData = await _moonphaseService.GetMoonPhaseAsync(date, RostockData.latitude, RostockData.longitude);
        return _moonphaseTranslationService.Translate(moonPhaseData.MoonPhase);
    }
}
