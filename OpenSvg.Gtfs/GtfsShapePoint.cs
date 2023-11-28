using OpenSvg.GeoJson;

namespace OpenSvg.Gtfs;
public record struct GtfsShapePoint(string ID, Coordinate Coordinate, int Sequence = 0, float Distance = 0)
{


}
