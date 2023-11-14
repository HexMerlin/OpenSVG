using OpenSvg.Attributes;

namespace OpenSvg.SvgNodes;

/// <summary>
/// Represents an SVG polygon element.
/// </summary>
public class SvgPolygon : SvgVisual
{
    public readonly PolygonAttr Polygon = new();

    public override string SvgName => SvgNames.Polygon;

    protected override ConvexHull ComputeConvexHull() => new(this.Polygon.Get());

}