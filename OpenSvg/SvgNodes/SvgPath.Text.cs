
using OpenSvg.Config;
using SkiaSharp;
using SkiaSharp.HarfBuzz;

namespace OpenSvg.SvgNodes;

/// <summary>
///     Represents an SVG Path element that renders a text with the specified styling.
/// </summary>
public partial class SvgPath : SvgVisual, IDisposable
{
    private static Path ConvertTextToSkPath(TextConfig textConfig) => ConvertTextToSkPath(textConfig.Text, textConfig.SvgFont.Font, (float)textConfig.FontSize);

    private static Path ConvertTextToSkPath(string text, SKTypeface typeFace, float fontSize)
    {
        using var paint = new SKPaint
        {
            Typeface = typeFace,
            TextSize = fontSize,
            IsAntialias = true
        };

        var path = new SKPath();
        SKFont font = typeFace.ToFont(fontSize);

        using var shaper = new SKShaper(typeFace);
        SKShaper.Result result = shaper.Shape(text, paint);

        for (int i = 0; i < result.Points.Length; i++)
        {
            SKPoint point = result.Points[i];
            int codePoint = (int)result.Codepoints[i];

            if (codePoint != 0)
            {
                using SKPath glyphPath = font.GetGlyphPath((ushort)codePoint);
                var matrix = SKMatrix.CreateTranslation(point.X, point.Y);
                glyphPath.Transform(matrix);
                path.AddPath(glyphPath);
            }
        }

        NormalizeToOrigin(path);
        return new Path(path);
    }

    private static void NormalizeToOrigin(SKPath path)
    {
        SKRect bounds = path.Bounds;
        path.Offset(-bounds.Left, -bounds.Top);
    }
}