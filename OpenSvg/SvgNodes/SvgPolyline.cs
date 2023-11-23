using OpenSvg.Attributes;

namespace OpenSvg.SvgNodes;

/// <summary>
/// Represents an SVG polygon element.
/// </summary>
public class SvgPolyline : SvgVisual
{
    protected readonly PolylineAttr polyline = new();


    public Polyline Polyline { get => this.polyline.Get(); set => this.polyline.Set(value); }

    /// <inheritdoc/>
    public override string SvgName => SvgNames.Polyline;

    /// <inheritdoc/>
    protected override ConvexHull ComputeConvexHull() => this.Polyline.ConvexHull;

}