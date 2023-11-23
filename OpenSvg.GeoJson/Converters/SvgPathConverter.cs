using GeoJSON.Net.Feature;
using OpenSvg.Config;
using OpenSvg.SvgNodes;

namespace OpenSvg.GeoJson.Converters;


public static class SvgPathConverter
{

    /// <summary>
    ///     Converts an <see cref="SvgPath"/> to a GeoJSON feature.
    /// </summary>
    /// <param name="svgPath">The <see cref="SvgPath"/> to convert.</param>
    /// <param name="parentTransform">The transformation set by the parent element.</param>
    /// <param name="converter">A converter for converting points and coordinates</param>
    /// <returns>The resulting GeoJSON feature.</returns>
    public static Feature ToFeature(this SvgPath svgPath, Transform parentTransform, PointConverter converter)
    {
        Transform composedTransform = parentTransform.ComposeWith(svgPath.Transform);
        MultiPolygon multiPolygon = svgPath.ApproximateToMultiPolygon(converter.SegmentCountForCurveApproximation);

        GeoJSON.Net.Geometry.MultiPolygon geoJsonMultiPolygon = multiPolygon.ToGeoJsonMultiPolygon(composedTransform, converter);

        var properties = svgPath.DrawConfig.ToDictionary();
        return new Feature(geoJsonMultiPolygon, properties);
    }


}
