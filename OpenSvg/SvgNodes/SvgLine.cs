using OpenSvg.Attributes;

namespace OpenSvg.SvgNodes;

/// <summary>
/// Represents an SVG line element.
/// </summary>
public class SvgLine : SvgVisual
{
    public readonly DoubleAttr X1 = new(SvgNames.X1);

    public readonly DoubleAttr X2 = new(SvgNames.X2);

    public readonly DoubleAttr Y1 = new(SvgNames.Y1);

    public readonly DoubleAttr Y2 = new(SvgNames.Y2);

    public override string SvgName => SvgNames.Line;

    public Point P1 => new(this.X1.Get(), this.Y1.Get());

    public Point P2 => new(this.X2.Get(), this.Y2.Get());

    //public override (bool Equal, string Message) CompareSelfAndDescendants(SvgElement other,
    //    double doublePrecision = Constants.DoublePrecision)
    //{
    //    if (ReferenceEquals(this, other)) return (true, "Same reference");
    //    (bool equal, string message) = base.CompareSelfAndDescendants(other);
    //    if (!equal)
    //        return (equal, message);
    //    var sameType = (SvgLine)other;
    //    if (this.X1 != sameType.X1)
    //        return (false, $"X1: {this.X1} != {sameType.X1}");
    //    if (this.Y1 != sameType.Y1)
    //        return (false, $"Y1: {this.Y1} != {sameType.Y1}");
    //    if (this.X2 != sameType.X2)
    //        return (false, $"X2: {this.X2} != {sameType.X2}");
    //    if (this.Y2 != sameType.Y2)
    //        return (false, $"Y2: {this.Y2} != {sameType.Y2}");
    //    return (true, "Equal");
    //}

    protected override ConvexHull ComputeConvexHull() => new(new[] { P1, P2 });
}