using System.Numerics;

namespace OpenSvg;

/// <summary>
///     Represents a point in a pixel coordinate system. The origin (0, 0) is at the top-left corner
/// </summary>
public readonly struct Point : IComparable<Point>, IEquatable<Point>
{
    public double X { get; }

    public double Y { get; }

    public Point(double x, double y)
    {
        this.X = x.Round();
        this.Y = y.Round();
    }

    public Point(Vector2 vector) : this(vector.X, vector.Y)
    {
    }

    /// <summary>
    ///     Gets the origin point at (0, 0).
    /// </summary>
    public static Point Origin => new(0, 0);

    public Vector2 AsVector => new((float)X, (float)Y);


    /// <summary>
    ///     Points are ordered by Y, then X.
    /// </summary>
    /// <example>
    ///     <para>
    ///         The following example demonstrates use of the <see cref="CompareTo" /> method.
    ///     </para>
    ///     <code><![CDATA[
    /// Point[] points = new Point[] { (0, 10), (20, 5) };
    /// Console.WriteLine(points.Min());
    /// // Output: (0, 10)
    /// ]]></code>
    /// </example>
    public int CompareTo(Point other) => (Y, X).CompareTo((other.Y, other.X));

    /// <summary>
    ///     Applies the specified transform to the point.
    /// </summary>
    /// <param name="transform">The transform to apply.</param>
    /// <returns>A new Point that has been transformed.</returns>
    public Point Transform(Transform transform) => new(Vector2.Transform(AsVector, transform.Matrix));

    /// <summary>
    ///     Determines whether this point is on the line segment between points a and b.
    /// </summary>
    /// <param name="a">The first point of the line segment.</param>
    /// <param name="b">The second point of the line segment.</param>
    /// <returns>True if this point is on the line segment, otherwise false.</returns>
    public readonly bool IsOnLineSegment(Point a, Point b)
    {
        const double Tolerance = 1e-6;

        // Check if point is within the bounding box of the line segment
        if (X < Math.Min(a.X, b.X) || X > Math.Max(a.X, b.X)) return false;
        if (Y < Math.Min(a.Y, b.Y) || Y > Math.Max(a.Y, b.Y)) return false;

        // Check if point is collinear with line segment using cross-product
        double crossProduct = (Y - a.Y) * (b.X - a.X) - (X - a.X) * (b.Y - a.Y);
        return Math.Abs(crossProduct) <= Tolerance;
    }

    public readonly bool Equals(Point other) => X.Equals(other.X) && Y.Equals(other.Y);

    public static implicit operator Point((double x, double y) tuple) => new(tuple.x, tuple.y);

    public static implicit operator (double X, double Y)(Point p) => (p.X, p.Y);

    /// <summary>
    ///     Returns a string that represents the point.
    /// </summary>
    /// <returns>A string representation of this object</returns>
    public override string ToString() => $"({Math.Round(X, 3).ToXmlString()}, {Math.Round(Y, 3).ToXmlString()})";

    public override bool Equals(object obj) => obj is Point other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(X, Y);

    public static bool operator ==(Point left, Point right) => left.Equals(right);

    public static bool operator !=(Point left, Point right) => !(left == right);

    public static bool operator <(Point left, Point right) => left.CompareTo(right) < 0;

    public static bool operator <=(Point left, Point right) => left.CompareTo(right) <= 0;

    public static bool operator >(Point left, Point right) => left.CompareTo(right) > 0;

    public static bool operator >=(Point left, Point right) => left.CompareTo(right) >= 0;
}