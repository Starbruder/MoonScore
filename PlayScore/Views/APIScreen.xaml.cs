using Microsoft.Extensions.DependencyInjection;
using MoonScore.DataConstants;
using MoonScore.Models;
using MoonScore.Services;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media.Imaging;

namespace MoonScore.Views;

/// <summary>
/// Interaction logic for APIScreen.xaml
/// </summary>
public sealed partial class APIScreen : Window
{
    private readonly IServiceProvider _serviceProvider;
    private readonly DatabaseManager _databaseManager;
    private readonly GameService _gameService;
    private readonly MoonphaseService _moonphaseService;
    private readonly MoonphaseTranslationService _moonPhaseTranslator;

    public ObservableCollection<GameModel> Games { get; } = [];

    public APIScreen(
        IServiceProvider serviceProvider,
        DatabaseManager databaseManager,
        GameService gameService,
        MoonphaseTranslationService moonphaseTranslationService,
        MoonphaseService moonphaseService)
    {
        InitializeComponent();

        _serviceProvider = serviceProvider;
        _databaseManager = databaseManager;
        _gameService = gameService;
        _moonPhaseTranslator = moonphaseTranslationService;
        _moonphaseService = moonphaseService;

        DateTextBox.Text = InitData.GetGamesInitDate();
    }
    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);
        Application.Current.Shutdown();
    }

    private void OpenMainWindow(object sender, RoutedEventArgs e)
    {
        _serviceProvider.GetRequiredService<MainWindow>().Show();
        this.Hide();
    }

    private async void GetMoonphase(object sender, RoutedEventArgs e)
    {
        string formattedDate = DateTimeService.FormatDateInput(DateTextBox.Text);

        try
        {
            var moonPhaseData = await _moonphaseService.GetMoonPhaseAsync(formattedDate, RostockData.latitude, RostockData.longitude);

            if (moonPhaseData is null)
            {
                MessageBox.Show("Failed to retrieve moon phase data. Please check your connection or try again.");
                return;
            }

            MoonPhaseTextBlock.Text = _moonPhaseTranslator.GetMoonPhaseData(moonPhaseData.MoonPhase).Name;

            var imageUri = new Uri("pack://application:,,,/Assets/Images/phases/8_FullMoon.png", UriKind.Absolute);
            moonImage.Source = new BitmapImage(imageUri);

            const string imagePath = "pack://application:,,,/Assets/Images/phases/";
            var imageService = new MoonphaseImageService(imagePath, ".png");
            var image = imageService.GetMoonPhaseImage(moonPhaseData.MoonPhase);
            moonImage.Source = image;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error getting moonphase: {ex.Message}");
        }
    }

    private async void GetGamesAsync(object sender, RoutedEventArgs e)
    {
        string formattedDate = DateTimeService.FormatDateInput(DateTextBox.Text);

        GamesListBox.ItemsSource = Games;

        try
        {
            var gameData = await _gameService.GetGamesByReleaseDateAsync(formattedDate);

            if (gameData is null)
            {
                MessageBox.Show("Failed to retrieve game data. Please check your connection or try again.");
                return;
            }

            if (gameData.Count == 0)
            {
                MessageBox.Show("Cannot show games for the requested day. There may not exist any game releases on that day.");
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
}
