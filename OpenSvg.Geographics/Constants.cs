using OpenSvg.Config;
using SkiaSharp;

namespace OpenSvg.Geographics;

public static class Constants
{
    internal const int CoordinateDecimalPrecision = 8;

    public static SKColor DefaultFillColor { get; } = SKColors.Transparent;
    public static SKColor DefaultStrokeColor { get; } = SKColors.Black;

    public static double DefaultStrokeWidth { get; } = 0;

    public static string TransparentColorString { get; } = "rgba(0, 0, 0, 0)";

    public static DrawConfig DefaultConfigPath { get; } = new DrawConfig(SKColors.Black, SKColors.Transparent, 0);

    public static DrawConfig DefaultConfigPolygon { get; } = new DrawConfig(SKColors.Transparent, SKColors.Black, 1);

    public static DrawConfig DefaultConfigLines { get; } = new DrawConfig(SKColors.Transparent, SKColors.Black, 1);

    public static DrawConfig DefaultConfigText { get; } = new DrawConfig(SKColors.Black, SKColors.Transparent, 0);
}
