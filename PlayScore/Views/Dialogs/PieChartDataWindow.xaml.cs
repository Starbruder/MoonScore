using MoonScore.Services;
using System.Windows;

namespace MoonScore.Views;

/// <summary>
/// Interaktionslogik für PieChartDataWindow.xaml
/// </summary>
public sealed partial class PieChartDataWindow : Window
{
    public PieChartDataWindow(DatabaseManager databaseManager)
    {
        InitializeComponent();

        var gamesPerMoonPhase = databaseManager.GetCountOfGamesPerMoonphase();

        var chartModel = ChartRenderer.CreatePiechartModel(gamesPerMoonPhase);
        MoonPhasePlot.Model = chartModel;
    }
}
