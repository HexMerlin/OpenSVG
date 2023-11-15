using OpenSvg.Attributes;

namespace OpenSvg.SvgNodes;


/// <summary>
/// Represents an SVG line element.
/// </summary>
public class SvgLine : SvgVisual
{
    /// <summary>
    /// Gets or sets the x-coordinate of the starting point of the line.
    /// </summary>
    public readonly DoubleAttr X1 = new(SvgNames.X1);

    /// <summary>
    /// Gets or sets the x-coordinate of the ending point of the line.
    /// </summary>
    public readonly DoubleAttr X2 = new(SvgNames.X2);

    /// <summary>
    /// Gets or sets the y-coordinate of the starting point of the line.
    /// </summary>
    public readonly DoubleAttr Y1 = new(SvgNames.Y1);

    /// <summary>
    /// Gets or sets the y-coordinate of the ending point of the line.
    /// </summary>
    public readonly DoubleAttr Y2 = new(SvgNames.Y2);

    /// <inheritdoc/>
    public override string SvgName => SvgNames.Line;

    /// <summary>
    /// Gets the starting point of the line.
    /// </summary>
    public Point P1 => new(this.X1.Get(), this.Y1.Get());

    /// <summary>
    /// Gets the ending point of the line.
    /// </summary>
    public Point P2 => new(this.X2.Get(), this.Y2.Get());

    /// <summary>
    /// Computes the convex hull of the line.
    /// </summary>
    /// <returns>The convex hull of the line.</returns>
    protected override ConvexHull ComputeConvexHull() => new(new[] { P1, P2 });
}
