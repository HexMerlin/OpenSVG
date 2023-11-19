using GeoJSON.Net.Feature;
using OpenSvg.Config;
using OpenSvg.SvgNodes;

namespace OpenSvg.GeoJson.Converters;


public static class SvgPathConverter
{

    /// <summary>
    /// Converts an SvgPath to a GeoJSON feature.
    /// </summary>
    /// <param name="svgPath">The SvgPath to convert.</param>
    /// <param name="transform">The transformation object for the SvgPath.</param>
    /// <returns>The resulting GeoJSON feature.</returns>
    /// 

    public static Feature ToFeature(this SvgPath svgPath, Transform parentTransform, PointConverter converter)
    {
        Transform composedTransform = parentTransform.ComposeWith(svgPath.Transform.Get());
        MultiPolygon multiPolygon = svgPath.ApproximateToMultiPolygon(converter.SegmentCountForCurveApproximation);

        GeoJSON.Net.Geometry.MultiPolygon geoJsonMultiPolygon = multiPolygon.ToGeoJsonMultiPolygon(composedTransform, converter);

        var properties = svgPath.DrawConfig.ToDictionary();
        return new Feature(geoJsonMultiPolygon, properties);
    }

}
