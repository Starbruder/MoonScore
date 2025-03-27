using OxyPlot.Series;
using OxyPlot;
using OxyPlot.Axes;

namespace MoonScore;

public static class ChartRenderer
{
    private const string ChartTitle = "Game Ratings vs Moon Phases";

    public static PlotModel CreateBarchartModel(IEnumerable<KeyValuePair<string, double>> keyValuePairs)
    {
        var plotModel = new PlotModel { Title = "Average " + ChartTitle };

        var categoryAxis = new CategoryAxis
        {
            Position = AxisPosition.Left,
            Title = "Moon Phase",
            IsZoomEnabled = false
        };

        categoryAxis.Labels.AddRange(keyValuePairs.Select(pair => pair.Key));
        plotModel.Axes.Add(categoryAxis);

        var valueAxis = new LinearAxis
        {
            Position = AxisPosition.Bottom,
            Title = "Average Rating",
            Minimum = 1,
            Maximum = 5,
            IsZoomEnabled = false
        };
        plotModel.Axes.Add(valueAxis);

        var labelFormatDecimals = "{0:0.000}";
        var barSeries = new BarSeries
        {
            LabelPlacement = LabelPlacement.Inside,
            LabelFormatString = labelFormatDecimals,
            FontSize = 12,
            LabelColor = OxyColors.Black
        };

        byte baseR = 103, baseG = 58, baseB = 183;

        uint index = 0; 

        foreach (var pair in keyValuePairs)
        {
            const byte maxValue = byte.MaxValue;
            byte r = (byte)Math.Min(baseR + index * 10, maxValue);  
            byte g = (byte)Math.Min(baseG + index * 12, maxValue);  
            byte b = (byte)Math.Min(baseB + index * 15, maxValue);  

            barSeries.Items.Add(new()
            {
                Value = pair.Value,
                Color = OxyColor.FromRgb(r, g, b),
            });

            index++; 
        }

        plotModel.Series.Add(barSeries);

        return plotModel;
    }

    public static PlotModel CreatePiechartModel(IEnumerable<KeyValuePair<string, long>> keyValuePairs)
    {
        var plotModel = new PlotModel
        {
            Title = "Amount of Games vs. Moon Phases",
            TextColor = OxyColor.FromRgb(r: 245, g: 245, b: 245),
            TitleFontSize = 20,
            TitlePadding = 10,
        };

        var pieSlices = new List<PieSlice>();

        byte baseR = 103, baseG = 58, baseB = 183;

        uint index = 0; 

        foreach (var pair in keyValuePairs)
        {
            const byte maxValue = byte.MaxValue;
            var r = (byte)Math.Min(baseR + index * 10, maxValue);  
            var g = (byte)Math.Min(baseG + index * 12, maxValue);  
            var b = (byte)Math.Min(baseB + index * 15, maxValue);  

            var pieSlice = new PieSlice(pair.Key, pair.Value)
            {
                Fill = OxyColor.FromRgb(r, g, b),
            };

            pieSlices.Add(pieSlice);

            index++;
        }

        var pieSeries = new PieSeries
        {
            StrokeThickness = 1,
            InsideLabelPosition = 0.77,
            Slices = pieSlices,
            InsideLabelFormat = "{1}",
            StartAngle = 251,
            InsideLabelColor = OxyColors.Black,
        };

        plotModel.Series.Add(pieSeries);

        return plotModel;
    }
}
