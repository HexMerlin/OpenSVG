using OpenSvg.Attributes;

namespace OpenSvg.SvgNodes;

public class SvgPolygon : SvgVisual
{
    public readonly PolygonAttr Polygon = new();

    public override string SvgName => SvgNames.Polygon;

    protected override ConvexHull ComputeConvexHull() => new(this.Polygon.Get());

    //public override (bool Equal, string Message) CompareSelfAndDescendants(SvgElement other,
    //    double doublePrecision = Constants.DoublePrecision)
    //{
    //    if (ReferenceEquals(this, other)) return (true, "Same reference");
    //    (bool equal, string message) = base.CompareSelfAndDescendants(other);
    //    if (!equal)
    //        return (equal, message);
    //    var sameType = (SvgPolygon)other;
    //    if (this.Polygon != sameType.Polygon)
    //        return (false, $"Polygon: {this.Polygon} != {sameType.Polygon}");

    //    return (true, "Equal");
    //}
}