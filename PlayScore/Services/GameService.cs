using Newtonsoft.Json;
using MoonScore.Models;
using System.Configuration;
using System.Net.Http;

namespace MoonScore.Services;

public sealed class GameService : IService
{
    private readonly HttpClient _httpClient;
    private readonly string apiKey = ConfigurationManager.AppSettings["API_KEY_GAMES"] ?? string.Empty;
    private readonly string ApiUrl;
    private readonly MoonphaseService _moonphaseService;
    private readonly MoonphaseTranslator _moonphaseTranslator;

    public GameService(MoonphaseService moonphaseService, MoonphaseTranslator moonphaseTranslator)
    {
        _httpClient = new HttpClient();
        ApiUrl = $"https://api.rawg.io/api/games?key={apiKey}&dates=";
        _moonphaseService = moonphaseService;
        _moonphaseTranslator = moonphaseTranslator;
    }

    public async Task<List<GameModel>> GetGamesByReleaseDateAsync(string releaseDate)
    {
        try
        {
            // Build the URL with the date filter, first date is start, second is end
            string url = $"{ApiUrl}{releaseDate},{releaseDate}";

            // Send a GET request
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            // Read the response content
            string content = await response.Content.ReadAsStringAsync();

            // Deserialize the JSON into a list of games
            var jsonResponse = JsonConvert.DeserializeObject<dynamic>(content);
            var games = new List<GameModel>();

            foreach (var game in jsonResponse.results)
            {
                string gameReleaseDate = game.released;
                double gameRating = game.rating;

                string moonPhaseName = null;

                if (gameRating > 0)
                {
                    moonPhaseName = await GetGameMoonPhaseAsync(gameReleaseDate);
                }

                games.Add(new GameModel
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
        var moonPhaseData = await _moonphaseService.GetMoonPhaseAsync(date, 54.0924, 12.1407);
        var translatedMoonPhase = _moonphaseTranslator.Translate(moonPhaseData.MoonPhase);
        return translatedMoonPhase;
    }
}
