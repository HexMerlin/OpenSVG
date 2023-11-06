//File SvgPath.Text.cs

using OpenSvg.Config;
using SkiaSharp;
using SkiaSharp.HarfBuzz;

namespace OpenSvg.SvgNodes;

/// <summary>
///     Represents an SVG Path element that renders a text with the specified styling.
/// </summary>
public sealed partial class SvgPath : SvgVisual, IDisposable
{
    private static string ConvertTextToSkPathData(TextConfig textConfig)
    {
        return ConvertTextToSkPath(textConfig).ToSvgPathData();
    }

    private static SKPath ConvertTextToSkPath(TextConfig textConfig)
    {
        return ConvertTextToSkPath(textConfig.Text, textConfig.SvgFont.Font, (float)textConfig.FontSize);
    }

    private static SKPath ConvertTextToSkPath(string text, SKTypeface typeFace, float fontSize)
    {
        using var paint = new SKPaint
        {
            Typeface = typeFace,
            TextSize = fontSize,
            IsAntialias = true
        };

        var path = new SKPath();
        var font = typeFace.ToFont(fontSize);

        using var shaper = new SKShaper(typeFace);
        var result = shaper.Shape(text, paint);

        for (var i = 0; i < result.Points.Length; i++)
        {
            var point = result.Points[i];
            var codePoint = (int)result.Codepoints[i];

            if (codePoint != 0)
            {
                using var glyphPath = font.GetGlyphPath((ushort)codePoint);
                var matrix = SKMatrix.CreateTranslation(point.X, point.Y);
                glyphPath.Transform(matrix);
                path.AddPath(glyphPath);
            }
        }

        NormalizeToOrigin(path);
        return path;
    }

    private static void NormalizeToOrigin(SKPath path)
    {
        var bounds = path.Bounds;
        path.Offset(-bounds.Left, -bounds.Top);
    }
}