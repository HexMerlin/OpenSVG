using OpenSvg.Geographics;

namespace OpenSvg.Gtfs;
public record struct GtfsShapePoint(string ID, Coordinate Coordinate, int Sequence, float ShapeDistTraveled)
{


}
