using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot;
using MoonScore.Models;
using MoonScore.Services;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media.Imaging;
using MoonScore.DataConstants;

namespace MoonScore;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly DatabaseManager _databaseManager;
    private readonly MoonphaseService _moonphaseService;
    private readonly MoonphaseTranslationService _moonPhaseTranslator;
    private readonly GameService _gameService;

    public ObservableCollection<GameModel> Games { get; } = [];

    public MainWindow(
        DatabaseManager databaseManager,
        MoonphaseService moonphaseService,
        MoonphaseTranslationService moonphaseTranslationService,
        GameService gameService)
    {
        InitializeComponent();

        WindowState = WindowState.Normal;

        _databaseManager = databaseManager;
        _moonphaseService = moonphaseService;
        _moonPhaseTranslator = moonphaseTranslationService;
        _gameService = gameService;

        MoonPhasePlot.Model = CreatePlotModel();
    }

    private async void GetMoonphase(object sender, RoutedEventArgs e)
    {
        string date = DateTextBox.Text;

        try
        {
            var moonPhaseData = await _moonphaseService.GetMoonPhaseAsync(date, RostockData.latitude, RostockData.longitude);

            if (moonPhaseData is null)
            {
                MessageBox.Show("Failed to retrieve moon phase data. Please check your connection or try again.");
                return;
            }

            MoonPhaseTextBlock.Text = $"Mondphase: {_moonPhaseTranslator.GetMoonPhaseData(moonPhaseData.MoonPhase)}";

            // Getting Moon-Image
            //const string imagePath = "pack://application:,,,/Assets/Images/phases/";
            //var imageService = new MoonphaseImageService(imagePath, ".png");
            //var image = imageService.GetMoonPhaseImage(/*moonPhaseData.MoonPhase*/"");
            //moonImage.Source = image;

            var imageUri = new Uri("pack://application:,,,/Assets/Images/phases/8_FullMoon.png", UriKind.Absolute);
            moonImage.Source = new BitmapImage(imageUri);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error getting moonphase: {ex.Message}");
        }
    }

    private async void GetGames(object sender, RoutedEventArgs e)
    {
        string date = DateTextBox.Text;
        GamesListBox.ItemsSource = Games;

        try
        {
            var gameData = await _gameService.GetGamesByReleaseDateAsync(date);

            if (gameData is null || gameData.Count == 0)
            {
                MessageBox.Show("Failed to retrieve game data. Please check your connection or try again.");
                return;
            }

            Games.Clear();
            gameData.ForEach(Games.Add);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error getting games: {ex.Message}");
        }
    }

    public async void SaveGamesToDatabaseAsync(object sender, RoutedEventArgs e)
    {
        var games = (ObservableCollection<GameModel>)GamesListBox.ItemsSource;

        if (games is null || games.Count == 0)
        {
            MessageBox.Show("No games to save.");
            return;
        }

        try
        {
            await _databaseManager.AddGamesToSpieleTableAsync(games);
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
            barSeries.Items.Add(new()
            {
                Value = rating
            });
        }

        plotModel.Series.Add(barSeries);

        return plotModel;
    }


    public async void AddAllGamesToDatabase(object sender, RoutedEventArgs e)
    {
        List<string> dateStrings = new List<string>
        {
            "2005-01-11", "2005-03-22", "2005-10-18", "2006-03-20", "2006-11-07",
            "2006-11-19", "2007-08-21", "2007-11-01", "2007-11-05", "2007-11-13",
            "2008-04-29", "2008-06-12", "2008-10-28", "2008-11-17", "2009-02-05",
            "2009-05-17", "2009-10-20", "2009-11-10", "2009-10-29", "2010-01-05",
            "2010-01-26", "2010-05-18", "2010-05-23", "2010-07-27", "2010-09-14",
            "2010-10-19", "2010-11-09", "2011-11-11", "2011-09-22", "2011-10-04",
            "2011-10-18", "2011-11-18", "2012-03-06", "2012-05-15", "2012-09-18",
            "2012-10-09", "2012-11-29", "2013-06-14", "2013-09-17", "2013-10-29",
            "2013-11-22", "2014-03-11", "2014-05-29", "2014-09-09", "2014-09-30",
            "2014-11-18", "2015-03-24", "2015-05-19", "2015-07-07", "2015-09-01",
            "2015-09-15", "2015-10-27", "2016-05-24", "2016-06-29", "2016-05-13",
            "2016-10-28", "2016-11-29", "2017-01-24", "2017-02-24", "2017-03-03",
            "2017-04-04", "2017-02-28", "2017-03-07", "2017-09-29", "2017-10-27",
            "2017-09-14", "2018-01-26", "2018-01-25", "2018-04-20", "2018-08-07",
            "2018-09-07", "2018-10-26", "2018-12-07", "2019-03-22", "2019-01-25",
            "2019-08-27", "2019-10-25", "2019-11-08", "2020-03-23", "2020-03-20",
            "2020-03-20", "2020-07-17", "2020-09-17", "2020-12-10", "2021-01-20",
            "2021-05-07", "2021-10-08", "2021-11-09", "2022-02-25", "2022-02-18",
            "2022-07-19", "2022-11-09", "2023-01-25", "2023-05-12", "2023-06-02",
            "2023-08-03", "2023-09-06", "2023-10-20", "2023-10-27", "2024-01-26",
            "2024-01-26", "2024-02-29"
        };

        foreach(string date in dateStrings)
        {
            try
            {
                // Fetch games for the current date
                var gameData = await _gameService.GetGamesByReleaseDateAsync(date);

                if (gameData is null || gameData.Count == 0)
                {
                    MessageBox.Show($"No games found for date {date}. Skipping...");
                    continue;
                }

                // Add the fetched games to the database
                await _databaseManager.AddGamesToSpieleTableAsync(gameData);
                MessageBox.Show($"Games from {date} have been successfully added to the database.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving or saving games for {date}: {ex.Message}");
            }
        }
    }
}