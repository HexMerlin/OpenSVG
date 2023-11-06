using OpenSvg;
using OpenSvg.Attributes;
using System.Xml.Serialization;

namespace OpenSvg.SvgNodes;

public class SvgPolygon : SvgVisual
{

    public override string SvgName => SvgNames.Polygon;


    public readonly PolygonAttr Polygon = new();

    protected override ConvexHull ComputeConvexHull() => new(Polygon.Get());

    public override (bool Equal, string Message) CompareSelfAndDescendants(SvgElement other, double doublePrecision = Constants.DoublePrecision)
    {
        if (ReferenceEquals(this, other)) return (true, "Same reference");
        var (equal, message) = base.CompareSelfAndDescendants(other);
        if (!equal)
            return (equal, message);
        SvgPolygon sameType = (SvgPolygon)other;
        if (Polygon != sameType.Polygon)
            return (false, $"Polygon: {Polygon} != {sameType.Polygon}");
       
        return (true, "Equal");
    }
}
