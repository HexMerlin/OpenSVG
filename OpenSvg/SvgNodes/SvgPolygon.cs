using OpenSvg.Attributes;

namespace OpenSvg.SvgNodes;

/// <summary>
/// Represents an SVG polygon element.
/// </summary>
public class SvgPolygon: SvgVisual
{
    protected readonly PolygonAttr polygon = new();

    public Polygon Polygon { get => this.polygon.Get(); set => this.polygon.Set(value); }
    /// <inheritdoc/>
    public override string SvgName => SvgNames.Polygon;

    /// <inheritdoc/>
    protected override ConvexHull ComputeConvexHull() => this.Polygon.ConvexHull;

}