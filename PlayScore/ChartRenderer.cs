using OxyPlot.Series;
using OxyPlot;
using OxyPlot.Axes;

namespace MoonScore;

public static class ChartRenderer
{
    public static PlotModel CreateBarChartModel(Dictionary<string, double> ratings)
    {
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
        var barSeries = new BarSeries
        {
            LabelPlacement = LabelPlacement.Inside,
            LabelFormatString = "{0:0.000}",  // Display one decimal place for the label
            FontSize = 12,  // Optional: set the label font size
            LabelColor = OxyColors.Black // Optional: set the label color};
        };

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

    public static PlotModel CreateMockPieChartModel()
    {
        var plotModel = new PlotModel { Title = "Mock Pie Chart" };

        var pieSeries = new PieSeries
        {
            Slices = {
                    new("Category A", 40) { IsExploded = true },
                    new("Category B", 30),
                    new("Category C", 20),
                    new("Category D", 10)
                },
            InsideLabelFormat = "{0}: {1}%"
        };

        plotModel.Series.Add(pieSeries);

        // Set the plot model for the PlotView
        //plotView.Model = plotModel;
        return plotModel;
    }
}
