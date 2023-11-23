using OpenSvg.Attributes;
using System.Numerics;

namespace OpenSvg.SvgNodes;


/// <summary>
/// Represents an SVG circle element.
/// </summary>
public class SvgCircle : SvgVisual
{

    protected readonly DoubleAttr x = new(SvgNames.CircleX);
    protected readonly DoubleAttr y = new(SvgNames.CircleY);
    protected readonly DoubleAttr radius = new(SvgNames.Radius);

    /// <summary>
    /// Gets or sets the x-coordinate of the circle center.
    /// </summary>
    public double X { get => x.Get(); set => x.Set(value); }

    /// <summary>
    /// Gets or sets the y-coordinate of the circle center.
    /// </summary>
    /// 
    public double Y { get => y.Get(); set => y.Set(value); }

    /// <summary>
    /// Gets or sets the radius of the circle.
    /// </summary>
    /// 
    public double Radius { get => radius.Get(); set => radius.Set(value); }

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
            double angle = 2 * Math.PI * i / pointCount;
            double x = Center.X + Radius * Math.Cos(angle);
            double y = Center.Y + Radius * Math.Sin(angle);
            points[i] = new Point(x, y);
        }
        return new ConvexHull(points);
    }

    

}
