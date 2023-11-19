using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using GeoJSON;

namespace OpenSvg.GeoJson.Converters;

public static class PointListConverter
{


    public static GeoJSON.Net.Geometry.Polygon ToGeoJsonPolygon(this Polygon polygon, Transform transform, PointConverter converter)
    {
        var positions = polygon.Select(svgPoint => converter.ToPosition(svgPoint, transform)).ToList();

        if (!positions.First().Equals(positions.Last()))
        {
            positions.Add(positions.First());
        }

        return new GeoJSON.Net.Geometry.Polygon(new[] { new LineString(positions) });
    }

    public static LineString ToLineString(this Polyline polyline, Transform transform, PointConverter converter)
    {
        var positions = polyline.Select(svgPoint => converter.ToPosition(svgPoint, transform)).ToList();

        return new LineString(positions);
    }

    public static Polygon ToPolygon(this GeoJSON.Net.Geometry.Polygon polygon, PointConverter converter)
    {
        throw new NotImplementedException();    
    }

    //public static PointList ToPointList(this LinearRing linearRing, PointConverter converter)
    //{
    //    var points = linearRing.Coordinates.Select(converter.ToPoint).Take(polygon); //remove the last point (it's a duplicate of the first)


    //    if (linearRing.IsClosed)
    //    {
    //        var points = linearRing.Coordinates.Select(converter.ToPoint).Take(linearRing.Count - 1); //remove the last point (it's a duplicate of the first)
    //        return new Polygon(points);
    //    }
    //    else
    //    {
    //        var points = linearRing.Coordinates.Select(converter.ToPoint);
    //        return new Polyline(points);
    //    }

    //}
}
