using ICSharpCode.Decompiler.TypeSystem;
using OpenSvg.GeoJson;
using OpenSvg.SvgNodes;

namespace OpenSvg.Gtfs;



public record GtfsTrip(string RouteID, string ServiceID, string TripID, string TripHeadsign, int DirectionID, string ShapeID)
{
  

}
