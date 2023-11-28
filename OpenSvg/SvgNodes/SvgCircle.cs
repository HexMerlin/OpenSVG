using OpenSvg.Attributes;
using System.Numerics;

namespace OpenSvg.SvgNodes;


/// <summary>
/// Represents an SVG circle element.
/// </summary>
public class SvgCircle : SvgVisual
{

    protected readonly FloatAttr x = new(SvgNames.CircleX);
    protected readonly FloatAttr y = new(SvgNames.CircleY);
    protected readonly FloatAttr radius = new(SvgNames.Radius);

    /// <summary>
    /// Gets or sets the x-coordinate of the circle center.
    /// </summary>
    public float X { get => x.Get(); set => x.Set(value); }

    /// <summary>
    /// Gets or sets the y-coordinate of the circle center.
    /// </summary>
    /// 
    public float Y { get => y.Get(); set => y.Set(value); }

    /// <summary>
    /// Gets or sets the radius of the circle.
    /// </summary>
    /// 
    public float Radius { get => radius.Get(); set => radius.Set(value); }

    /// <inheritdoc/>
    public override string SvgName => SvgNames.Circle;

    /// <summary>
    /// Gets or sets the center point of the circle.
    /// </summary>
    public Point Center 
    { 
        get => new(X, Y); 
        set { X = value.X; Y = value.Y; }
    }


    /// <summary>
    /// Computes the convex hull of the line.
    /// </summary>
    /// <returns>The convex hull of the line.</returns>
    protected override ConvexHull ComputeConvexHull()
    {
        const int pointCount = 16; 
        var points = new Point[pointCount];
        for (int i = 0; i < points.Length; i++)
        {
            float angle = 2 * MathF.PI * i / pointCount;
            float x = Center.X + Radius * MathF.Cos(angle);
            float y = Center.Y + Radius * MathF.Sin(angle);
            points[i] = new Point(x, y);
        }
        return new ConvexHull(points);
    }

    

}
