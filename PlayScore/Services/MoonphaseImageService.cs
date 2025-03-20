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

    public Uri GetMoonPhaseImageUri(string moonPhaseImageName)
    {
        //var image = _moonPhaseImages.TryGetValue(moonPhaseImageName, out var moonPhaseImage)
        //    ? moonPhaseImage
        //    : "NOT_FOUND"; // Return the "NotFound"-Image if no translation is found.

        var image = "8_FullMoon";

        var fileName = imagePath + image + fileType;
        return new(fileName, UriKind.Absolute);
    }

    public BitmapImage GetMoonPhaseImage(string moonPhaseImageName)
    {
        var imageUri = GetMoonPhaseImageUri(moonPhaseImageName);
        return new(imageUri);
    }
}
