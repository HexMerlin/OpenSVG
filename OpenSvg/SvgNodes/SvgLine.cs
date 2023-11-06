using OpenSvg.Attributes;

namespace OpenSvg.SvgNodes;

public class SvgLine : SvgVisual
{
    public readonly DoubleAttr X1 = new(SvgNames.X1);

    public readonly DoubleAttr X2 = new(SvgNames.X2);

    public readonly DoubleAttr Y1 = new(SvgNames.Y1);

    public readonly DoubleAttr Y2 = new(SvgNames.Y2);

    public override string SvgName => SvgNames.Line;

    public Point P1 => new(X1.Get(), Y1.Get());

    public Point P2 => new(X2.Get(), Y2.Get());

    public override (bool Equal, string Message) CompareSelfAndDescendants(SvgElement other,
        double doublePrecision = Constants.DoublePrecision)
    {
        if (ReferenceEquals(this, other)) return (true, "Same reference");
        var (equal, message) = base.CompareSelfAndDescendants(other);
        if (!equal)
            return (equal, message);
        var sameType = (SvgLine)other;
        if (X1 != sameType.X1)
            return (false, $"X1: {X1} != {sameType.X1}");
        if (Y1 != sameType.Y1)
            return (false, $"Y1: {Y1} != {sameType.Y1}");
        if (X2 != sameType.X2)
            return (false, $"X2: {X2} != {sameType.X2}");
        if (Y2 != sameType.Y2)
            return (false, $"Y2: {Y2} != {sameType.Y2}");
        return (true, "Equal");
    }

    protected override ConvexHull ComputeConvexHull()
    {
        return new ConvexHull(new[] { P1, P2 });
    }
}