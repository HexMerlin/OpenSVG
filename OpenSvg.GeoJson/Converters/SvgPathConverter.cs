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
        Transform composedTransform = parentTransform.ComposeWith(svgPath.Transform.Get());
        MultiPolygon multiPolygon = svgPath.ApproximateToMultiPolygon(converter.SegmentCountForCurveApproximation);

        GeoJSON.Net.Geometry.MultiPolygon geoJsonMultiPolygon = multiPolygon.ToGeoJsonMultiPolygon(composedTransform, converter);

        var properties = svgPath.DrawConfig.ToDictionary();
        return new Feature(geoJsonMultiPolygon, properties);
    }


    /// <summary>
    /// Converts a GeoJSON feature to an <see cref="SvgGroup"/> of <see cref="SvgPolygon"/> objects.
    /// </summary>
    /// <param name="feature">The GeoJSON feature to convert.</param>
    /// <param name="converter">A converter for converting points and coordinates</param>
    /// <returns>The resulting <see cref="SvgGroup"/> of polygons.</returns>
    public static SvgGroup ToSvgPolygonGroup(this Feature feature, PointConverter converter)
    {
        throw new NotImplementedException("THIS METHOD SHOULD NOT EXIST - CONVERT TO PATH INSTEAD");
        //if (feature.Geometry is not GeoJSON.Net.Geometry.MultiPolygon geoJsonMultiPolygon)
        //    throw new ArgumentException($"Feature must have a MultiPolygon geometry to convert to {nameof(SvgPath)}.");

        //MultiPolygon multiPolygon = geoJsonMultiPolygon.ToMultiPolygon(converter);

        //SvgGroup svgGroup = multiPolygon.ToSvgPolygonGroup();
      
        //return svgGroup;
    }

}
