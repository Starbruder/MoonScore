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
    private readonly DatabaseHelper _databaseHelper;

    private readonly MoonphaseService _moonphaseService = new();
    private readonly GameService _gameService = new();

    private readonly string _dbPath = ConfigurationManager.AppSettings["CONNECTION_NAME"] ?? string.Empty;

    public ObservableCollection<GameModel> Games { get; } = [];

    public MainWindow(DatabaseManager databaseManager, DatabaseHelper databaseHelper)
    {
        InitializeComponent();

        _databaseManager = databaseManager;
        _databaseHelper = databaseHelper;
    }

    private void ConnectToDatabase(object sender, RoutedEventArgs e)
    {
        _databaseHelper.ConnectToDatabase();
        CreateDatabase(sender, e);
    }

    private void CreateDatabase(object sender, RoutedEventArgs e)
    {
        DatabaseManager.CreateDatabase(_dbPath);
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