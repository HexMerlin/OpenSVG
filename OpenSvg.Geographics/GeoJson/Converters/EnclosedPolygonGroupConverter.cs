using GeoJSON.Net.Geometry;
using OpenSvg.SvgNodes;

namespace OpenSvg.Geographics.GeoJson.Converters;


public static class EnclosedPolygonGroupConverter
{
    public static GeoJSON.Net.Geometry.Polygon ToGeoJsonPolygon(this EnclosedPolygonGroup group, Transform transform, PointConverter converter)
    {

        LineString exteriorLineString = group.ExteriorPolygon.ToLineString(transform, converter);

        IEnumerable<LineString> interiorLineStrings = group.InteriorPolygons.Select(polygon => polygon.ToLineString(transform, converter));
     
        return new GeoJSON.Net.Geometry.Polygon(new LineString[] { exteriorLineString }.Concat(interiorLineStrings));
    }

    private static LineString ToLineString(this Polygon polygon, Transform transform, PointConverter converter)
    {
        var positions = polygon.Select(svgPoint =>  converter.ToCoordinate(svgPoint.Transform(transform)).ToPosition()).ToList();
        positions.Add(positions.First()); // close the polygon
        return new LineString(positions);
    }

    public static SvgPath ToSvgPath(this GeoJSON.Net.Geometry.Polygon polygon, PointConverter converter)
    {
        if (polygon.Coordinates.Count == 0)
            throw new ArgumentException("Polygon must have at least a LineString object to convert to EnclosedPolygonGroup.");

        EnclosedPolygonGroup enclosedPolygonGroup = polygon.ToEnclosedPolygonGroup(converter);
        return enclosedPolygonGroup.ToPath().ToSvgPath();
    }

    public static EnclosedPolygonGroup ToEnclosedPolygonGroup(this GeoJSON.Net.Geometry.Polygon polygon, PointConverter converter)
    {
        LineString lineString = polygon.Coordinates.First();

        Polygon exteriorPolygon = lineString.ToPolygon(converter);

        var interiorPolygons = polygon.Coordinates.Skip(1).Select(lineString => lineString.ToPolygon(converter)).ToList();
              
        var enclosedPolygonGroup = new EnclosedPolygonGroup(exteriorPolygon, interiorPolygons);
    
        return enclosedPolygonGroup;
    }
}


