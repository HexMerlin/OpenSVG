using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using GeoJSON;

namespace OpenSvg.GeoJson.Converters;

public static class MultiPolygonConverter
{

    public static GeoJSON.Net.Geometry.MultiPolygon ToGeoJsonMultiPolygon(this MultiPolygon multiPolygon, Transform transform, PointConverter converter)
    {
        var polygons = multiPolygon.Select(group => group.ToGeoJsonPolygon(transform, converter)).ToList();

        return new GeoJSON.Net.Geometry.MultiPolygon(polygons);
    }

}
