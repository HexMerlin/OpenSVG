using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using GeoJSON;

namespace OpenSvg.GeoJson.Converters;

public static class PolylineConverter
{

    public static LineString ToLineString(this Polyline polyline, Transform transform, PointConverter converter)
    {
        IEnumerable<Position> positions = polyline.Select(svgPoint => converter.ToPosition(svgPoint, transform));

        return new LineString(positions);
    }

    public static Polyline ToPolyline(this LineString lineString, PointConverter converter)
    {
        var points = lineString.Coordinates.Select(converter.ToPoint).ToList();
        return new Polyline(points);
    }
}
