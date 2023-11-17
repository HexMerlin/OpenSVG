using OpenSvg.Attributes;

namespace OpenSvg.SvgNodes;

/// <summary>
/// Represents an SVG polygon element.
/// </summary>
public class SvgPolygon: SvgVisual
{
    public readonly PolygonAttr Polygon = new();

    /// <inheritdoc/>
    public override string SvgName => SvgNames.Polygon;

    /// <inheritdoc/>
    protected override ConvexHull ComputeConvexHull() => this.Polygon.Get().ConvexHull;

}