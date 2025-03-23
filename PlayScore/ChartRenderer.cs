using OxyPlot.Series;
using OxyPlot;
using OxyPlot.Axes;
using System.Buffers.Text;
using System;

namespace MoonScore;

public static class ChartRenderer
{
    private const string ChartTitle = "Game Ratings vs Moon Phases";

    public static PlotModel CreateBarchartModel(Dictionary<string, double> ratings)
    {
        var plotModel = new PlotModel { Title = ChartTitle };

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
            LabelColor = OxyColors.Black // Optional: set the label color
        };

        foreach (var rating in ratings.Values)
        {
            barSeries.Items.Add(new()
            {
                Value = rating,
                Color = OxyColor.FromRgb(143, 80, 255),
            });
        }

        plotModel.Series.Add(barSeries);

        return plotModel;
    }

    public static PlotModel CreatePiechartModel(Dictionary<string, double> ratings)
    {
        var plotModel = new PlotModel { Title = "Pie Chart with Different Shades" };

        var pieSlices = new List<PieSlice>();

        // Base color (RGB 103, 58, 183)
        byte baseR = 103;
        byte baseG = 58;
        byte baseB = 183;

        int index = 0; // Used to modify shades

        foreach (var rating in ratings)
        {
            // Generate a slightly different shade for each slice
            byte r = (byte)Math.Min(baseR + index * 10, 255);  // Increase Red slightly
            byte g = (byte)Math.Min(baseG + index * 12, 255);  // Increase Green slightly
            byte b = (byte)Math.Min(baseB + index * 15, 255);  // Increase Blue slightly

            var pieSlice = new PieSlice(rating.Key, rating.Value)
            {
                // Tried adding/setting slightly diffrent color shades for each slice
                Fill = OxyColor.FromRgb(r, g, b),
            };

            pieSlices.Add(pieSlice);

            index++; // Increment index for next slice
        }

        // Create the PieSeries and add slices
        var pieSeries = new PieSeries
        {
            Slices = pieSlices,
            InsideLabelFormat = "{0:0.00}: {1}",
            StrokeThickness = 1
        };

        plotModel.Series.Add(pieSeries);

        return plotModel;
    }

    private static PlotModel CreateMockPiechartModel()
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

        return plotModel;
    }

    private static Dictionary<string, double> CreateBarchartMockDataRatings()
    {
        // Generate sample data, still need method for getting the data from our database
        var ratings = new Dictionary<string, double>
            {
                { "New Moon", 7.5 },
                { "First Quarter", 8.2 },
                { "Full Moon", 9.1 },
                { "Last Quarter", 6.8 }
            };

        return ratings;
    }
}
