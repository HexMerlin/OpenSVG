using GeoJSON.Net.Geometry;

namespace OpenSvg.GeoJson.Converters;

public static class PolygonConverter
{


    public static GeoJSON.Net.Geometry.Polygon ToGeoJsonPolygon(this Polygon polygon, Transform transform, PointConverter converter) 
        => new GeoJSON.Net.Geometry.Polygon(new[] { polygon.ToLineString(transform, converter) });


    public static LineString ToLineString(this Polygon polygon, Transform transform, PointConverter converter)
    {
        var positions = polygon.Select(svgPoint => converter.ToPosition(svgPoint, transform)).ToList();
        positions.Add(positions.First()); // close the polygon
        return new LineString(positions);
    }

    public static Polygon ToPolygon(this GeoJSON.Net.Geometry.Polygon polygon, PointConverter converter)
    {
        LineString lineString = polygon.Coordinates.First();
        return lineString.ToPolygon(converter);
    }

    public static Polygon ToPolygon(this LineString lineString, PointConverter converter)
    {
        var points = lineString.Coordinates.Select(converter.ToPoint).ToList();
        points.RemoveAt(points.Count - 1); // remove the last point, which is the same as the first
        return new Polygon(points);
    }
}
