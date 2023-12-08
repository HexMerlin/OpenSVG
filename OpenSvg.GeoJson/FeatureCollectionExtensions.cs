using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using OpenSvg.GeoJson;
using OpenSvg.GeoJson.Converters;
using GeoPoint = GeoJSON.Net.Geometry.Point;

public static class FeatureCollectionExtensions
{

    public static GeoBoundingBox GetBoundingBox(this FeatureCollection featureCollection) 
        => new GeoBoundingBox(featureCollection.GetCoordinates());

    public static IEnumerable<Coordinate> GetCoordinates(this FeatureCollection featureCollection)
        => featureCollection.Features.SelectMany(f => f.Geometry.GetCoordinates());

    private static IEnumerable<Coordinate> GetCoordinates(this IGeometryObject geometry) 
        => geometry switch
    {
        GeoPoint point => point.GetCoordinates(),
        MultiPoint multiPoint => multiPoint.GetCoordinates(),
        LineString lineString => lineString.GetCoordinates(),
        MultiLineString multiLineString => multiLineString.GetCoordinates(),
        Polygon polygon => polygon.GetCoordinates(),
        MultiPolygon multiPolygon => multiPolygon.GetCoordinates(),
        GeometryCollection geometryCollection => geometryCollection.GetCoordinates(),
        _ => throw new NotSupportedException($"Unsupported GeoJSON geometry type {geometry.GetType().Name}"),
    };


    private static IEnumerable<Coordinate> GetCoordinates(this GeometryCollection geometryCollection) 
        => geometryCollection.Geometries.SelectMany(g => g.GetCoordinates());

    private static IEnumerable<Coordinate> GetCoordinates(this GeoPoint geoPoint) { yield return geoPoint.Coordinates.ToCoordiate();}
    private static IEnumerable<Coordinate> GetCoordinates(this MultiPoint multiPoint) => multiPoint.Coordinates.Select(p => p.Coordinates).Select(c => c.ToCoordiate());

    private static IEnumerable<Coordinate> GetCoordinates(this LineString lineString) => lineString.Coordinates.Select(c => c.ToCoordiate());


    private static IEnumerable<Coordinate> GetCoordinates(this Polygon polygon) => polygon.Coordinates.SelectMany(ring => ring.Coordinates).Select(c => c.ToCoordiate());

    private static IEnumerable<Coordinate> GetCoordinates(this MultiLineString multiLineString) => multiLineString.Coordinates.SelectMany(line => line.Coordinates).Select(c => c.ToCoordiate());

    private static IEnumerable<Coordinate> GetCoordinates(this MultiPolygon multiPolygon) => multiPolygon.Coordinates.SelectMany(poly => poly.Coordinates.SelectMany(ring => ring.Coordinates)).Select(c => c.ToCoordiate());

}
