using GeoJSON.Net.Geometry;
using GeoJSON.Net.Feature;
using OpenSvg.Config;
using OpenSvg.SvgNodes;

namespace OpenSvg.GeoJson.Converters;


public static class SvgPolygonConverter
{

    public static Feature ToFeature(this SvgPolygon svgPolygon, Transform parentTransform, PointConverter converter)
    {
        Transform composedTransform = parentTransform.ComposeWith(svgPolygon.Transform.Get());
        Polygon polygon = svgPolygon.Polygon.Get();

        GeoJSON.Net.Geometry.Polygon geoJsonPolygon = polygon.ToGeoJsonPolygon(composedTransform, converter);

        var properties = svgPolygon.DrawConfig.ToDictionary();
        return new Feature(geoJsonPolygon, properties);
    }



}
