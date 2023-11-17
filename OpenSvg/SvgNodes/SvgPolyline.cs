using OpenSvg.Attributes;

namespace OpenSvg.SvgNodes;

/// <summary>
/// Represents an SVG polygon element.
/// </summary>
public class SvgPolyline : SvgVisual
{
    public readonly PolylineAttr Polyline = new();

    /// <inheritdoc/>
    public override string SvgName => SvgNames.Polyline;

    /// <inheritdoc/>
    protected override ConvexHull ComputeConvexHull() => this.Polyline.Get().ConvexHull;

}