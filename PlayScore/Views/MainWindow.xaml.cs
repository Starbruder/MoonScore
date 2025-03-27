using MoonScore.Models;
using MoonScore.Services;
using System.Collections.ObjectModel;
using System.Windows;
using MoonScore.Views;
using Microsoft.Extensions.DependencyInjection;

namespace MoonScore;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly IServiceProvider _serviceProvider;
    private readonly DatabaseManager _databaseManager;

    public MainWindow(IServiceProvider serviceProvider, DatabaseManager databaseManager)
    {
        InitializeComponent();

        _serviceProvider = serviceProvider;
        _databaseManager = databaseManager;
    }

    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);
        Application.Current.Shutdown(); 
    }

    private void Barchart_Click(object sender, RoutedEventArgs e)
    {
        new BarChartDataWindow(_databaseManager).Show();
    }

    private void Piechart_Click(object sender, RoutedEventArgs e)
    {
        new PieChartDataWindow(_databaseManager).Show();
    }

    private void OpenAPIScreen(object sender, RoutedEventArgs e)
    {
        var apiScreen = _serviceProvider.GetRequiredService<APIScreen>();
        apiScreen.Show();
        this.Hide();
    }

    private void ShowMoonPhases_Click(object sender, RoutedEventArgs e)
    {
        new MoonPhasesOverviewWindow().Show();
    }
}
