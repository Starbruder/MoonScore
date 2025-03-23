using OxyPlot.Series;
using OxyPlot;
using OxyPlot.Axes;
using System.Globalization;

namespace MoonScore;

public static class ChartRenderer
{
    private const string ChartTitle = "Game Ratings vs Moon Phases";

    public static PlotModel CreateBarchartModel(IEnumerable<KeyValuePair<string, double>> keyValuePairs)
    {
        var plotModel = new PlotModel { Title = ChartTitle };

        // Define the Y-Axis (Categories for Moon Phases)
        var categoryAxis = new CategoryAxis
        {
            Position = AxisPosition.Left, // BarChart uses Left Y-Axis for categories
            Title = "Moon Phase",
            IsZoomEnabled = false
        };

        categoryAxis.Labels.AddRange(keyValuePairs.Select(pair => pair.Key));
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

        // Base color (RGB 103, 58, 183)
        byte baseR = 103;
        byte baseG = 58;
        byte baseB = 183;

        int index = 0; // Used to modify shades

        foreach (var pair in keyValuePairs)
        {
            // Generate a slightly different shade for each slice
            byte r = (byte)Math.Min(baseR + index * 10, 255);  // Increase Red slightly
            byte g = (byte)Math.Min(baseG + index * 12, 255);  // Increase Green slightly
            byte b = (byte)Math.Min(baseB + index * 15, 255);  // Increase Blue slightly

            barSeries.Items.Add(new()
            {
                Value = pair.Value,
                Color = OxyColor.FromRgb(r, g, b),
                //Color = OxyColor.FromRgb(143, 80, 255),
            });

            index++; // Increment index for next slice
        }

        plotModel.Series.Add(barSeries);

        return plotModel;
    }

    public static PlotModel CreatePiechartModel(IEnumerable<KeyValuePair<string, long>> keyValuePairs)
    {
        var plotModel = new PlotModel
        {
            Title = ChartTitle,
            TextColor = OxyColor.FromRgb(245, 245, 245),
            TitleFontSize = 20,
            TitlePadding = 10,
        };

        var pieSlices = new List<PieSlice>();

        // Base color ( RGB: 103, 58, 183 )
        byte baseR = 103, baseG = 58, baseB = 183;

        int index = 0; // Used to modify shades

        foreach (var pair in keyValuePairs)
        {
            // Generate a slightly different shade for each slice
            byte r = (byte)Math.Min(baseR + index * 10, 255);  // Increase Red slightly
            byte g = (byte)Math.Min(baseG + index * 12, 255);  // Increase Green slightly
            byte b = (byte)Math.Min(baseB + index * 15, 255);  // Increase Blue slightly

            var pieSlice = new PieSlice(pair.Key, pair.Value)
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
            StrokeThickness = 1,
            InsideLabelPosition = 0.85, // Controls positioning of labels, inside the pie
            AngleSpan = 330,
            Slices = pieSlices,
            InsideLabelFormat = "{0}: {1}",
            StartAngle = 251,
            InsideLabelColor = OxyColors.Black,
        };

        plotModel.Series.Add(pieSeries);

        return plotModel;
    }

    private static PlotModel CreateMockPiechartModel()
    {
        var plotModel = new PlotModel { Title = "Mock Pie Chart" };

        var mockData = CreateMockDataRatings();
        
        var pieSeries = new PieSeries
        {
            Slices = {
                    new(mockData[0].Key, mockData[0].Value) { IsExploded = true },
                    new(mockData[1].Key, mockData[1].Value),
                    new(mockData[2].Key, mockData[2].Value),
                    new(mockData[3].Key, mockData[3].Value)
                },
            InsideLabelFormat = "{0}: {1}%"
        };

        plotModel.Series.Add(pieSeries);

        return plotModel;
    }

    private static KeyValuePair<string, double>[] CreateMockDataRatings()
    {
        // Generate sample data, still need method for getting the data from our database
        var ratings = new KeyValuePair<string, double>[]
            {
                new("New Moon", 7.5),
                new("First Quarter", 8.2),
                new("Full Moon", 9.1),
                new("Last Quarter", 6.8)
            };

        return ratings;
    }
}
