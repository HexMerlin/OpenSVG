using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using OpenSvg.Config;
using OpenSvg.SvgNodes;
using SkiaSharp;

namespace OpenSvg.GeoJson.Converters;


public static class SvgLineConverter
{

    /// <summary>
    ///     Converts an SvgLine to a GeoJSON feature.
    /// </summary>
    /// <param name="svgLine">The SvgLine to convert.</param>
    /// <param name="parentTransform">The transformation set by the parent element.</param>
    /// <param name="converter">A converter for converting points and coordinates</param>
    /// <returns>The resulting GeoJSON feature.</returns>
    public static Feature ToFeature(this SvgLine svgLine, Transform parentTransform, PointConverter converter)
    {
        Transform composedTransform = parentTransform.ComposeWith(svgLine.Transform.Get());

        Position p1 = converter.ToPosition(svgLine.P1, composedTransform);
        Position p2 = converter.ToPosition(svgLine.P2, composedTransform);

        LineString lineString = new LineString(new List<Position> { p1, p2 });

        var properties = svgLine.DrawConfig.ToDictionary();
     
        return new Feature(lineString, properties);
    }

    /// <summary>
    /// Converts a GeoJSON feature to an <see cref="SvgLine"/>.
    /// </summary>
    /// <param name="feature">The GeoJSON feature to convert.</param>
    /// <param name="converter">A converter for converting points and coordinates</param>
    /// <returns>The resulting <see cref="SvgLine"/>.</returns>
    public static SvgLine ToSvgLine(this Feature feature, PointConverter converter)
    {
        if (feature.Geometry is not LineString lineString || lineString.Coordinates.Count != 2)
            throw new ArgumentException($"Feature must have a LineString geometry with exactly two points to convert to {nameof(SvgLine)}.");

        var p1 = converter.ToPoint(lineString.Coordinates.First());
        var p2 = converter.ToPoint(lineString.Coordinates.Last());

        Dictionary<string, object>? properties = feature.Properties as Dictionary<string, object>;
        DrawConfig drawConfig = properties?.ToDrawConfig() ?? new DrawConfig();

        var svgLine = new SvgLine
        {
            P1 = p1,
            P2 = p2,
            DrawConfig = drawConfig,
        };

        return svgLine;
    }

}



