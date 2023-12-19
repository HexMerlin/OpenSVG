using OpenSvg.Geographics;
using OpenSvg.SvgNodes;


namespace OpenSvg.Gtfs;

public record GtfsStopTime(string TripID, string ArrivalTime, string DepartureTime, string StopID, int StopSequence, string StopHeadsign, string PickupType, string DropOffType, float ShapeDistTraveled, string Timepoint)
{
  

}
