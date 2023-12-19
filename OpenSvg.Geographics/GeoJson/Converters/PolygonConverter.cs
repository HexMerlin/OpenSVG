using GeoJSON.Net.Geometry;
using OpenSvg.SvgNodes;

namespace OpenSvg.Geographics.GeoJson.Converters;

public static class PolygonConverter
{


    public static GeoJSON.Net.Geometry.Polygon ToGeoJsonPolygon(this Polygon polygon, Transform transform, PointConverter converter)
    {
        var positions = polygon.Select(svgPoint => converter.ToCoordinate(svgPoint, transform).ToPosition()).ToList();
        positions.Add(positions.First()); // close the polygon
        LineString lineString = new LineString(positions);
        return new GeoJSON.Net.Geometry.Polygon(new[] { lineString });
    }



    public static Polygon ToPolygon(this GeoJSON.Net.Geometry.Polygon polygon, PointConverter converter)
    {
        if (polygon.Coordinates.Count != 1)
            throw new ArgumentException($"Polygon must have exactly one LineString to convert to {nameof(Polygon)}.");

        LineString lineString = polygon.Coordinates.First();
        return lineString.ToPolygon(converter);
    }

    public static Polygon ToPolygon(this LineString lineString, PointConverter converter)
    {
        IEnumerable<Point> points = lineString.Coordinates.Select(c => converter.ToPoint(c.ToCoordiate())).SkipLast(1); // remove the last point, which is the same as the first
        return new Polygon(points);
    }
}
