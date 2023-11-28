using ICSharpCode.Decompiler.CSharp.Transforms;
using ICSharpCode.Decompiler.TypeSystem;
using OpenSvg.GeoJson;
using OpenSvg.SvgNodes;

namespace OpenSvg.Gtfs;
public record GtfsStop(string StopID, string Name, Coordinate Coordinate, int LocationType = 0, string ParentStation = "", string PlatformCode = "")
{
    public SortedDictionary<string, GtfsShape> Shapes { get; } = new SortedDictionary<string, GtfsShape>();

    public void AddShape(GtfsShape shape)
    {
        Shapes[shape.ID] = shape;
    }

}
