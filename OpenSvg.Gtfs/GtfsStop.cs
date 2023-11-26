using ICSharpCode.Decompiler.TypeSystem;
using OpenSvg.GeoJson;
using OpenSvg.SvgNodes;

namespace OpenSvg.Gtfs;
public readonly record struct GtfsStop(string ID, string Name, Coordinate Coordinate, int LocationType = 0, string ParentStation = "", string PlatformCode = "")
{
  

}
