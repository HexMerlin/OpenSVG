using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using GeoJSON;
using HarfBuzzSharp;
using OpenSvg.SvgNodes;

namespace OpenSvg.GeoJson.Converters;


public static class EnclosedPolygonGroupConverter
{
    public static GeoJSON.Net.Geometry.Polygon ToGeoJsonPolygon(this EnclosedPolygonGroup group, Transform transform, PointConverter converter)
    {

        LineString exteriorLineString = group.ExteriorPolygon.ToLineString(transform, converter);

        IEnumerable<LineString> interiorLineStrings = group.InteriorPolygons.Select(polygon => polygon.ToLineString(transform, converter));
     
        return new GeoJSON.Net.Geometry.Polygon(new LineString[] { exteriorLineString }.Concat(interiorLineStrings));
    }


    public static EnclosedPolygonGroup ToEnclosedPolygonGroup(this GeoJSON.Net.Geometry.Polygon polygon, PointConverter converter)
    {
        if (!polygon.Coordinates.Any())
            throw new ArgumentException("Polygon must have at least one LineString to convert to EnclosedPolygonGroup.");

        LineString lineString = polygon.Coordinates.First();

        Polygon exteriorPolygon = lineString.ToPolygon(converter);

        var interiorPolygons = polygon.Coordinates.Skip(1).Select(lineString => lineString.ToPolygon(converter)).ToList();
              
        var enclosedPolygonGroup = new EnclosedPolygonGroup(exteriorPolygon, interiorPolygons);
    
        return enclosedPolygonGroup;
    }
}


