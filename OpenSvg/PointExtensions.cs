
using System.Numerics;

namespace OpenSvg;

/// <summary>
/// Provides extension methods for manipulating points.
/// </summary>
public static class PointExtensions
{

    /// <summary>
    /// Transforms the point by the specified transform.
    /// </summary>
    /// <param name="point">The point to transform.</param>
    /// <param name="transform">The transform to apply to the point.</param>
    /// <returns>The transformed point.</returns>
    public static Point Transform(this Point point, Transform transform) => Vector2.Transform(point, transform.Matrix);

    /// <summary>
    /// Determines whether the point is on a line segment defined by two points.
    /// </summary>
    /// <param name="p">The point to check.</param>
    /// <param name="a">The start point of the line segment.</param>
    /// <param name="b">The end point of the line segment.</param>
    /// <returns>True if the point is on the line segment; otherwise, false.</returns>
    public static bool IsOnLineSegment(this Point p, Point a, Point b)
    {
        const float Tolerance = 1e-5f;

        // Check if point is within the bounding box of the line segment
        if (p.X < MathF.Min(a.X, b.X) || p.X > MathF.Max(a.X, b.X)) return false;
        if (p.Y < MathF.Min(a.Y, b.Y) || p.Y > MathF.Max(a.Y, b.Y)) return false;

        // Check if point is collinear with line segment using cross-product
        float crossProduct = (p.Y - a.Y) * (b.X - a.X) - (p.X - a.X) * (b.Y - a.Y);
        return MathF.Abs(crossProduct) <= Tolerance;
    }

    /// <summary>
    /// Compares a current point with another point, primarily based on their Y coordinates, followed by their X coordinates.
    /// Ordering is similar to how text is read in an English document: left to right, then top to bottom.
    /// </summary>
    /// <remarks>
    /// The comparison is first made using the Y coordinates. If the Y coordinates are equal,
    /// the comparison is then made using the X coordinates. 
    /// </remarks>
    /// <param name="other">The point to compare to this instance.</param>
    /// <returns>
    /// A signed integer that indicates the relative order of the points being compared.
    /// The return value has these meanings: 
    /// Less than zero: This instance precedes 'other' in the sort order.
    /// Zero: This instance occurs in the same position in the sort order as 'other'.
    /// Greater than zero: This instance follows 'other' in the sort order.
    /// </returns>
    public static int CompareTo(this Point p1, Point p2)
    {
        int yComparison = p1.Y.CompareTo(p2.Y);
        if (yComparison != 0)
            return yComparison;

        return p1.X.CompareTo(p2.X);
    }
}
