using GeoJSON.Net.Geometry;

namespace OpenSvg.Geographics.GeoJson.Converters;

public static class PolylineConverter
{

    public static LineString ToLineString(this Polyline polyline, Transform transform, PointConverter converter)
    {
        IEnumerable<Position> positions = polyline.Select(svgPoint => converter.ToCoordinate(svgPoint, transform).ToPosition());

        return new LineString(positions);
    }

    public static Polyline ToPolyline(this LineString lineString, PointConverter converter)
    {
        var points = lineString.Coordinates.Select(c => converter.ToPoint(c.ToCoordiate())).ToList();
        return new Polyline(points);
    }
}
