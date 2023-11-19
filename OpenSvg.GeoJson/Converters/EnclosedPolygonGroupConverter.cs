using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using GeoJSON;

namespace OpenSvg.GeoJson.Converters;


public static class EnclosedPolygonGroupConverter
{
    public static GeoJSON.Net.Geometry.Polygon ToGeoJsonPolygon(this EnclosedPolygonGroup group, Transform transform, PointConverter converter)
    {

        LineString exteriorLineString = group.ExteriorPolygon.ToLineString(transform, converter);

        IEnumerable<LineString> interiorLineStrings = group.InteriorPolygons.Select(polygon => polygon.ToLineString(transform, converter));
     
        return new GeoJSON.Net.Geometry.Polygon(new LineString[] { exteriorLineString }.Concat(interiorLineStrings));
    }
}


