﻿using MoonScore.Models;
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

        DrawRatingsBarChart();
    }

    private void DrawRatingsBarChart()
    {
        var ratings = _databaseManager.GetAverageRatingPerMondphase();

        var chartModel = ChartRenderer.CreateBarChartModel(ratings);
        MoonPhasePlot.Model = chartModel;
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

            MoonPhaseTextBlock.Text = $"Mondphase: {_moonPhaseTranslator.Translate(moonPhaseData.MoonPhase)}";

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

    private async void GetGamesAsync(object sender, RoutedEventArgs e)
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
}