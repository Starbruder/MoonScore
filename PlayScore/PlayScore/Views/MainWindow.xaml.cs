using System.Collections.ObjectModel;
using System.Configuration;
using System.Windows;
using PlayScore.Models;
using PlayScore.Services;

namespace PlayScore;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly DatabaseManager _databaseManager;

    private readonly MoonphaseService _moonphaseService;
    private readonly GameService _gameService;

    public ObservableCollection<GameModel> Games { get; } = [];

    public MainWindow(DatabaseManager databaseManager, MoonphaseService moonphaseService, GameService gameService)
    {
        InitializeComponent();

        WindowState = WindowState.Maximized;

        _databaseManager = databaseManager;
        _moonphaseService = moonphaseService;
        _gameService = gameService;
    }

    private async void GetMoonphase(object sender, RoutedEventArgs e)
    {
        string date = DateTextBox.Text;

        // Example: Rostock 
        var latitude = 54.0924;
        var longitude = 12.1407;

        var moonPhaseData = await _moonphaseService.GetMoonPhaseAsync(date, latitude, longitude);

        if (moonPhaseData == null)
        {
            MessageBox.Show("Failed to retrieve moon phase data. Please check your connection or try again.");
            return;
        }

        var translator = new MoonphaseTranslator();
        MoonPhaseTextBlock.Text = $"Mondphase: {translator.Translate(moonPhaseData.MoonPhase)}";
    }

    private async void GetGames(object sender, RoutedEventArgs e)
    {
        string date = DateTextBox.Text;
        GamesListBox.ItemsSource = Games;

        var gameData = await _gameService.GetGamesByReleaseDateAsync(date);

        if (gameData == null)
        {
            MessageBox.Show("Failed to retrieve game data. Please check your connection or try again.");
            return;
        }

        Games.Clear();
        gameData.ForEach(Games.Add);
    }

    public void SaveGamesToDatabase(object sender, RoutedEventArgs e)
    {
        var games = (ObservableCollection<GameModel>)GamesListBox.ItemsSource;
        foreach(var game in games)
        {
            if(game.Rating > 0)
            {
                _databaseManager.AddGameToSpieleTable(game);
            }
        }
    }
}