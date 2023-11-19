using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using OpenSvg.SvgNodes;

namespace OpenSvg.GeoJson.Converters;


public static class SvgPolylineConverter
{


    public static Feature ToFeature(this SvgPolyline svgPolyline, Transform parentTransform, PointConverter converter)
    {
        Transform composedTransform = parentTransform.ComposeWith(svgPolyline.Transform.Get());
        Polyline polyline = svgPolyline.Polyline.Get();

        LineString lineString = polyline.ToLineString(composedTransform, converter);

        var properties = svgPolyline.DrawConfig.ToDictionary();

        return new Feature(lineString, properties);
    }



}
