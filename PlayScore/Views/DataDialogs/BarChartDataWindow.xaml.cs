using MoonScore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MoonScore.Views;

/// <summary>
/// Interaktionslogik für BarChartDataWindow.xaml
/// </summary>
public sealed partial class BarChartDataWindow : Window
{
    private readonly DatabaseManager _databaseManager;

    public BarChartDataWindow(
        DatabaseManager databaseManager)
    {
        InitializeComponent();

        _databaseManager = databaseManager;

        DrawRatingsBarChart();
    }

    private void DrawRatingsBarChart()
    {
        var ratings = _databaseManager.GetAverageRatingPerMondphase();

        var chartModel = ChartRenderer.CreateBarchartModel(ratings);
        MoonPhasePlot.Model = chartModel;
    }
}
