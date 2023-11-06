using SkiaSharp;

namespace OpenSvg.Config;

/// <summary>
///     Record used to configure drawing parameters for an SVG shape.
/// </summary>
/// <remarks>
///     Provides properties to set the fill color, stroke color, and stroke width for SVG shapes.
/// </remarks>
public record DrawConfig(SKColor FillColor, SKColor StrokeColor, double StrokeWidth)
{
    /// <summary>
    ///     Gets a transparent DrawConfig instance.
    ///     One use-case is for creating invisible spaces between shapes.
    /// </summary>
    public static DrawConfig Transparent => new(SKColors.Transparent, SKColors.Transparent, 0);

    /// <summary>
    ///     Returns a new copy of the DrawConfig record with the specified fill color.
    /// </summary>
    /// <param name="fillColor">The new fill color to set.</param>
    /// <returns>A new DrawConfig with the specified fill color.</returns>
    public DrawConfig WithFillColor(SKColor fillColor)
    {
        return this with { FillColor = fillColor };
    }

    /// <summary>
    ///     Returns a new copy of the DrawConfig record with the specified stroke color.
    /// </summary>
    /// <param name="strokeColor">The new stroke color to set.</param>
    /// <returns>A new DrawConfig with the specified stroke color.</returns>
    public DrawConfig WithStrokeColor(SKColor strokeColor)
    {
        return this with { StrokeColor = strokeColor };
    }
}