using SkiaSharp;

namespace OpenSvg.GeoJson;

public class Constants
{
    internal const int CoordinateDecimalPrecision = 8;

    public static SKColor DefaultFillColor { get; } = SKColors.Transparent;
    public static SKColor DefaultStrokeColor { get; } = SKColors.Black;

    public static double DefaultStrokeWidth { get; } = 1;

    public static string TransparentColorString { get; } = "rgba(0, 0, 0, 0)";
}
