

using System.Drawing;

namespace OpenSvg;

/// <summary>
/// Represents a bounding box defined by two points.
/// </summary>
/// <param name="UpperLeft">The UpperLeft point.</param>
/// <param name="LowerRight">The LowerRight point.</param>
public readonly struct BoundingBox
{
    public BoundingBox()
    {
        this.UpperLeft = Point.Origin;
        this.LowerRight = Point.Origin;
    }

    public BoundingBox(Point upperLeft, Point lowerRight)
    {
        this.UpperLeft = upperLeft;
        this.LowerRight = lowerRight;
    }

    public readonly Point UpperLeft;

    public readonly Point LowerRight;
    /// <summary>
    /// A bounding box starting at (0,0) thas has zero size.
    /// </summary>
    public static readonly BoundingBox None = new(Point.Origin, Point.Origin);

    /// <summary>
    /// Gets the size of the bounding box.
    /// </summary>
    public readonly Size Size => new(LowerRight.X - UpperLeft.X, LowerRight.Y - UpperLeft.Y);

    /// <summary>
    /// Gets the minimum x coordinate of the bounding box.
    /// </summary>
    public readonly double MinX => UpperLeft.X;

    /// <summary>
    /// Gets the midpoint of the x coordinates of the bounding box.
    /// </summary>
    public readonly double MidX => (MinX + MaxX) / 2;

    /// <summary>
    /// Gets the maximum x coordinate of the bounding box.
    /// </summary>
    public readonly double MaxX => LowerRight.X;

    /// <summary>
    /// Gets the minimum y coordinate of the bounding box.
    /// </summary>
    public readonly double MinY => UpperLeft.Y;

    /// <summary>
    /// Gets the midpoint of the y coordinates of the bounding box.
    /// </summary>
    public readonly double MidY => (MinY + MaxY) / 2;

    /// <summary>
    /// Gets the maximum y coordinate of the bounding box.
    /// </summary>
    public readonly double MaxY => LowerRight.Y;

    /// <summary>
    /// Calculates the minimum bounding box that contains both this bounding box and another bounding box.
    /// </summary>
    /// <param name="other">The other bounding box.</param>
    /// <returns>The union bounding box.</returns>
    public readonly BoundingBox UnionWith(BoundingBox other) => new(new Point(double.Min(MinX, other.MinX), double.Min(MinY, other.MinY)), new Point(double.Max(MaxX, other.MaxX), double.Max(MaxY, other.MaxY)));

    /// <summary>
    /// Checks if this bounding box intersects with another bounding box.
    /// </summary>
    /// <param name="other">The other bounding box.</param>
    /// <returns>True if the bounding boxes intersect, false otherwise.</returns>
    public readonly bool Intersects(BoundingBox other) => !(MaxX < other.MinX || MinX > other.MaxX || MaxY < other.MinY || MinY > other.MaxY);

    public override string ToString() => $"Upper left: {UpperLeft}, Lower right: {LowerRight}";
}