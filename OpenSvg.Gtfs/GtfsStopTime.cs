using ICSharpCode.Decompiler.TypeSystem;
using OpenSvg.GeoJson;
using OpenSvg.SvgNodes;


namespace OpenSvg.Gtfs;

public record GtfsStopTime(string TripID, string ArrivalTime, string DepartureTime, string StopID, int StopSequence, string StopHeadsign, string PickupType, string DropOffType, float shapeDistTraveled, string Timepoint)
{
  

}
