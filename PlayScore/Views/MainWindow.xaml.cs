using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot;
using MoonScore.Models;
using MoonScore.Services;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media.Imaging;

namespace MoonScore;

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

        WindowState = WindowState.Normal;

        _databaseManager = databaseManager;
        _moonphaseService = moonphaseService;
        _gameService = gameService;

        MoonPhasePlot.Model = CreatePlotModel();
    }

    private async void GetMoonphase(object sender, RoutedEventArgs e)
    {
        string date = DateTextBox.Text;

        // Example: Rostock 
        const double latitude = 54.0924;
        const double longitude = 12.1407;

        var moonPhaseData = await _moonphaseService.GetMoonPhaseAsync(date, latitude, longitude);

        if (moonPhaseData == null)
        {
            MessageBox.Show("Failed to retrieve moon phase data. Please check your connection or try again.");
            return;
        }

        var translator = new MoonphaseTranslator();
        MoonPhaseTextBlock.Text = $"Mondphase: {translator.Translate(moonPhaseData.MoonPhase)}";

        // Getting Moon-Image
        //const string imagePath = "pack://application:,,,/Assets/Images/phases/";
        //var imageService = new MoonphaseImageService(imagePath, ".png");
        //var image = imageService.GetMoonPhaseImage(/*moonPhaseData.MoonPhase*/"");
        //moonImage.Source = image;

        var imageUri = new Uri("pack://application:,,,/Assets/Images/phases/8_FullMoon.png", UriKind.Absolute);
        moonImage.Source = new BitmapImage(imageUri);
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

    //public async void SaveGamesToDatabase(object sender, RoutedEventArgs e)
    //{
    //    var games = (ObservableCollection<GameModel>)GamesListBox.ItemsSource;
    //    foreach (var game in games)
    //    {
    //        if (game.Rating > 0)
    //        {
    //            await _databaseManager.AddGameToSpieleTableAsync(game);
    //        }
    //    }
    //}

    public async void SaveGamesToDatabaseAsync(object sender, RoutedEventArgs e)
    {
        try
        {
            var games = (ObservableCollection<GameModel>)GamesListBox.ItemsSource;

            if (games == null || !games.Any())
            {
                MessageBox.Show("No games to save.");
                return;
            }

            foreach (var game in games)
            {
                if (game.Rating > 0)
                {
                    await _databaseManager.AddGameToSpieleTableAsync(game);
                }
            }

            MessageBox.Show("Games saved successfully!");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error saving games: {ex.Message}");
        }
    }

    private PlotModel CreatePlotModel()
    {
        var ratings = _databaseManager.GetAverageRatingPerMondphase();

        // Generate sample data, still need method for getting the data from our database
        //Dictionary<string, double> ratings = new Dictionary<string, double>
        //    {
        //        { "New Moon", 7.5 },
        //        { "First Quarter", 8.2 },
        //        { "Full Moon", 9.1 },
        //        { "Last Quarter", 6.8 }
        //    };

        var plotModel = new PlotModel { Title = "Game Ratings vs Moon Phases" };

        // Define the Y-Axis (Categories for Moon Phases)
        var categoryAxis = new CategoryAxis
        {
            Position = AxisPosition.Left, // BarChart uses Left Y-Axis for categories
            Title = "Moon Phase",
            IsZoomEnabled = false
        };

        categoryAxis.Labels.AddRange(ratings.Keys);
        plotModel.Axes.Add(categoryAxis);

        // Define the X-Axis (Ratings)
        var valueAxis = new LinearAxis
        {
            Position = AxisPosition.Bottom,
            Title = "Average Rating",
            Minimum = 0,
            Maximum = 5,
            IsZoomEnabled = false
        };
        plotModel.Axes.Add(valueAxis);

        // Add BarSeries
        var barSeries = new BarSeries { LabelPlacement = LabelPlacement.Inside };

        foreach (var rating in ratings.Values)
        {
            barSeries.Items.Add(new BarItem { Value = rating });
        }

        plotModel.Series.Add(barSeries);

        return plotModel;
    }
}