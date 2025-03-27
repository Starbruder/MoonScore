using System.Windows.Media.Imaging;

namespace MoonScore.Services;

public sealed class MoonphaseImageService(string imagePath, string fileType) : IService
{
    private readonly Dictionary<string, string> _moonPhaseImages = new()
    {
        { "NEW_MOON", "0_NewMoon" },
        { "WAXING_CRESCENT", "14_WaxingCrescent" },
        { "FIRST_QUARTER", "12_FirstQuarter" },
        { "WAXING_GIBBOUS", "10_WaxingGibbous" },
        { "FULL_MOON", "8_FullMoon" },
        { "WANING_GIBBOUS", "6_WaningGibbous" },
        { "LAST_QUARTER", "4_WaningGibbous" },
        { "WANING_CRESCENT", "2_WaningGibbous" },
        { "NOT_FOUND", "16_NotFound" }
    };

    private string BuildFilenameString(string image) => (imagePath + image + fileType);

    public Uri GetMoonPhaseImageUri(string moonPhaseImageName)
    {
        if (!_moonPhaseImages.TryGetValue(key: moonPhaseImageName, value: out var image))
        {
            image = "16_NotFound";
        }

        string fileName = BuildFilenameString(image);
        return new(fileName, UriKind.Absolute);
    }

    public BitmapImage GetMoonPhaseImage(string moonPhaseImageName)
    {
        var imageUri = GetMoonPhaseImageUri(moonPhaseImageName);
        return new(imageUri);
    }
}
