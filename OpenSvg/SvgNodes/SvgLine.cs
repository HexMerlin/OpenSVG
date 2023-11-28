using OpenSvg.Attributes;

namespace OpenSvg.SvgNodes;


/// <summary>
/// Represents an SVG line element.
/// </summary>
public class SvgLine : SvgVisual
{
    protected readonly FloatAttr x1 = new(SvgNames.X1);
    protected readonly FloatAttr x2 = new(SvgNames.X2);
    protected readonly FloatAttr y1 = new(SvgNames.Y1);
    protected readonly FloatAttr y2 = new(SvgNames.Y2);

    /// <summary>
    /// Gets or sets the x-coordinate of the starting point of the line.
    /// </summary>
    public float X1 { get => x1.Get(); set => x1.Set(value); }

    /// <summary>
    /// Gets or sets the x-coordinate of the ending point of the line.
    /// </summary>
    public float X2 { get => x2.Get(); set => x2.Set(value); }

    /// <summary>
    /// Gets or sets the y-coordinate of the starting point of the line.
    /// </summary>
    public float Y1 { get => y1.Get(); set => y1.Set(value); }

    /// <summary>
    /// Gets or sets the y-coordinate of the ending point of the line.
    /// </summary>
    public float Y2 { get => y2.Get(); set => y2.Set(value); }

    /// <inheritdoc/>
    public override string SvgName => SvgNames.Line;

    /// <summary>
    /// Gets or sets the starting point of the line.
    /// </summary>
    public Point P1 
    { 
        get => new(X1, Y1); 
        set { X1 = value.X; Y1 = value.Y; }
    }

    /// <summary>
    /// Gets or sets the ending point of the line.
    /// </summary>
    public Point P2
    {
        get => new(X2, Y2);
        set { X2 = value.X; Y2 = value.Y; }
    }

    /// <summary>
    /// Computes the convex hull of the line.
    /// </summary>
    /// <returns>The convex hull of the line.</returns>
    protected override ConvexHull ComputeConvexHull() => new(new[] { P1, P2 });
}
