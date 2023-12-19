using GeoJSON.Net.Feature;
using OpenSvg.SvgNodes;

namespace OpenSvg.Geographics.GeoJson.Converters;

public static class SvgPolygonConverter
{
    /// <summary>
    ///     Converts an <see cref="SvgPolygon"/> to a GeoJSON feature.
    /// </summary>
    /// <param name="svgPolygon">The SvgPolygon to convert.</param>
    /// <param name="parentTransform">The transformation set by the parent element.</param>
    /// <param name="converter">A converter for converting points and coordinates</param>
    /// <returns>The resulting GeoJSON feature.</returns>
    public static Feature ToFeature(this SvgPolygon svgPolygon, Transform parentTransform, PointConverter converter)
    {
        Transform composedTransform = parentTransform.ComposeWith(svgPolygon.Transform);
        Polygon polygon = svgPolygon.Polygon;

        GeoJSON.Net.Geometry.Polygon geoJsonPolygon = polygon.ToGeoJsonPolygon(composedTransform, converter);

        var properties = svgPolygon.DrawConfig.ToDictionary();
        return new Feature(geoJsonPolygon, properties);
    }

    /// <summary>
    /// Converts a GeoJSON feature to an <see cref="SvgPolygon"/>.
    /// </summary>
    /// <param name="feature">The GeoJSON feature to convert.</param>
    /// <param name="converter">A converter for converting points and coordinates</param>
    /// <returns>The resulting <see cref="SvgPolygon"/>.</returns>
    public static SvgPolygon ToSvgPolygon(this Feature feature, PointConverter converter)
    {
        if (feature.Geometry is not GeoJSON.Net.Geometry.Polygon geoJsonPolygon || geoJsonPolygon.Coordinates.Count != 1)
            throw new ArgumentException($"Feature must have a Polygon geometry with exactly one LineString to convert to {nameof(SvgPolygon)}.");

        Polygon polygon = geoJsonPolygon.ToPolygon(converter);  

        SvgPolygon svgPolygon = new SvgPolygon();
        svgPolygon.Polygon = polygon;
 
        return svgPolygon;
    }

}
